using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Threading;


namespace NPMGui
{
    public struct Repo
    {
        public string name;
        public string url;
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        JObject jsonFile;
        List<string> packagesInstalled = new List<string>();
        
        /// <summary>
        /// Reads a json file
        /// </summary>
        /// <param name="path">File to read</param>
        /// <returns>JObject with parsed json</returns>
        JObject ReadJsonFile(string path)
        {
            string json = "";
            using (StreamReader reader = new StreamReader(path))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    json += line;
                }
            }

            return JObject.Parse(json);
        }

        /// <summary>
        /// Convert all dependencies in a json file to ssh instead of https
        /// </summary>
        /// <param name="obj">JSON object</param>
        /// <param name="fileName">Path to json file to write to</param>
        void WriteAllDeps(JObject obj, string fileName)
        {
            textBox1.Clear();
            JObject deps = (JObject)obj["dependencies"];
            if (deps != null)
            {
                foreach (var item in deps.Properties())
                {
                    string package = item.Name;
                    string repo = GetRepo(package);
                    if (repo == null)
                        continue;
                    string old = (string)deps[package];
                    deps[package] = ((string)(repo));
                    textBox1.AppendText($"Dependency: {package} being changed from: {old} to {deps[package]}\n");
                }
            }

            

            JObject devDeps = (JObject)obj["devDependencies"];
            if (devDeps == null)
                return;

            foreach (var item in devDeps.Properties())
            {
                string package = item.Name;
                string repo = GetRepo(package);
                if (repo == null)
                    continue;
                string old = (string)devDeps[package];
                devDeps[package] = ((string)(repo));
                textBox1.AppendText($"Dev Dependency: {package} being changed from: {old} to {devDeps[package]}\n");
            }
            using (StreamWriter file = new StreamWriter(fileName))
            {
                using (JsonTextWriter jw = new JsonTextWriter(file))
                {
                    jw.Formatting = Formatting.Indented;
                    obj.WriteTo(jw);
                }
            }
            textBox1.AppendText($"Operation complete\n");
        }

        /// <summary>
        /// Get all dependencies from a package.json
        /// </summary>
        /// <param name="json">Package.json file</param>
        /// <returns>List of all repos/dependencies</returns>
        List<Repo> GetAllDeps(JObject json)
        {
            JObject deps = (JObject)json["dependencies"];
            List<Repo> repos = new List<Repo>();
            if (deps == null)
                return null;

            foreach (var item in deps.Properties())
            {
                Repo r;
                r.name = item.Name.ToString();
                r.url = item.Value.ToString();

                repos.Add(r);
            }
            return repos;
        }

        /// <summary>
        /// Get json representation of an npm package
        /// </summary>
        /// <param name="package">Package name</param>
        /// <returns>A json object of the package</returns>
        JObject GetNPMPackage(string package)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://registry.npmjs.org/{package}");
            request.Method = "Get";
            request.ContentType = "application/json";

            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string json = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    json = sr.ReadToEnd();
                }
                return JObject.Parse(json);
            }
            catch
            {
                return null;
            }          
        }

        /// <summary>
        /// Get a dependency from the package and change it's url to ssh
        /// </summary>
        /// <param name="package">Package.json</param>
        /// <returns>String of new url</returns>
        string GetRepo(string package)
        {
            JObject obj = GetNPMPackage(package);
            if (obj == null)
                return null;

            if (obj["repository"] == null)
                return null;

            if (obj["repository"]["url"] == null)
                return null;

            string url = (string)obj["repository"]["url"];
            if (url.Contains("git+https"))
            {
                url = url.Replace("git+https", "git+ssh");
            }
            else if (url.Contains("git://"))
            {
                url = url.Replace("git://", "git+ssh://");
            }
            else
            {
                url = url.Replace("https", "ssh");
            }

            url = url.Replace("github.com", "git@github.com");


            return url;
        }

        /// <summary>
        /// Get an npm dependency and find/download all dependencies it has
        /// </summary>
        /// <param name="repo">Repo to clone</param>
        /// <param name="folder">Folder to download to</param>
        void CloneRepo(Repo repo, string folder, string nodeModulesFolder)
        {
            if (packagesInstalled.Contains(repo.name))
            {
                gitTextBox.AppendText($"{repo.name} already installed, skipping...\n\n");
                return;
            }

            packagesInstalled.Add(repo.name);

            gitTextBox.AppendText($"Installing repo {repo.name} in folder {folder}...\n\n");
            DownloadTarball(repo.name, nodeModulesFolder);

            DirectoryInfo dir = new DirectoryInfo($"{nodeModulesFolder}\\{repo.name}");
            string packageLocation = dir + "\\package.json";
            if (File.Exists(packageLocation))
            {
                JObject package = ReadJsonFile(packageLocation);
                WriteAllDeps(package, packageLocation);
                List<Repo> repos = GetAllDeps(package);
                if(repos != null)
                {
                    foreach (Repo newRepo in repos)
                    {
                        CloneRepo(newRepo, dir.FullName, nodeModulesFolder);
                    }
                }     
                // TODO: also install dev dependencies?
            }
        }

        /// <summary>
        /// Download the zipped tarball of the npm package
        /// </summary>
        /// <param name="package">Package to download</param>
        /// <param name="folder">Folder to download to</param>
        void DownloadTarball(string package, string folder)
        {
            JObject json = GetNPMPackage(package);
            string latest = json["dist-tags"]["latest"].ToString();
            if(versionTextBox.Text != "")
            {
                latest = versionTextBox.Text;
            }
            if(json["versions"][latest] == null)
            {
                throw new Exception("Unknown package version");
            }
            string file = json["versions"][latest]["dist"]["tarball"].ToString();
            using (var client = new WebClient())
            {
                gitTextBox.AppendText($"Downloading {package} version {latest}\n\n");
                if(package.Contains("/"))
                    package = package.Split('/')[1];

                string outputFile = $"{folder}\\{package}.tgz";
                client.DownloadFile(file, outputFile);

                Chilkat.Gzip gzip = new Chilkat.Gzip();
                bool success;
                success = gzip.UnlockComponent("Anything for 30-day trial");
                if (!success)
                {
                    MessageBox.Show(gzip.LastErrorText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                success = gzip.UnTarGz($"{folder}\\{package}.tgz", folder, true);
                if (!success)
                {
                    MessageBox.Show(gzip.LastErrorText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                File.Delete($"{folder}\\{package}");
                File.Delete($"{folder}\\{package}.tgz");
                if(Directory.Exists($"{folder}\\package"))
                    Directory.Move($"{folder}\\package", $"{folder}\\{package}");
            }
        }
        

        /// <summary>
        /// Install the base repo from the npm package
        /// </summary>
        /// <param name="package">Package to get</param>
        /// <param name="folder">Folder to download to</param>
        void InstallBaseRepo(string package, string folder)
        {
            packagesInstalled.Clear();
            string url = GetRepo(package);
            if (url == null)
                throw new Exception("Package not found");


            DownloadTarball(package, folder);
        }

        private void gitBashBtn_Click(object sender, EventArgs e)
        {
            gitTextBox.Clear();
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {                  
                    InstallBaseRepo(repoTextBox.Text, folderBrowserDialog1.SelectedPath);
                    if (repoTextBox.Text.Contains("/"))
                        repoTextBox.Text = repoTextBox.Text.Split('/')[1];
                    string fileName = $"{folderBrowserDialog1.SelectedPath}\\{repoTextBox.Text}\\package.json";
                    jsonFile = ReadJsonFile(fileName);
                    WriteAllDeps(jsonFile, fileName);
                    List<Repo> repos = GetAllDeps(jsonFile);
                    Directory.CreateDirectory($"{folderBrowserDialog1.SelectedPath}\\node_modules");
                    if (repos != null)
                    {                        
                        foreach (Repo repo in repos)
                        {
                            CloneRepo(repo, $"{folderBrowserDialog1.SelectedPath}\\{repoTextBox.Text}", $"{folderBrowserDialog1.SelectedPath}\\node_modules");
                        }
                    }
                    Directory.Move($"{folderBrowserDialog1.SelectedPath}\\{repoTextBox.Text}", $"{folderBrowserDialog1.SelectedPath}\\node_modules\\{repoTextBox.Text}");
                    gitTextBox.AppendText("Operation complete\n");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            else
            {
                MessageBox.Show("Failed to open folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
