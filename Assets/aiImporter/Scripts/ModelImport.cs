using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using DataLib;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using ProtoBuf;
using System;
using System.Runtime.InteropServices;

namespace UnityImporter
{
    public class ModelImport : MonoBehaviour
    {

        /**
		File name that we want aiImporter to generate for us
		We will look for this file and load it once it exists
		It is "scene.bin" by default, change it if you want to load multiple files and want to distinguish them
		**/
        public string lookFor = "scene.bin";

        private GameScene unityScene = new GameScene();

        private GameObject mainObject;

        // Use this for initialization
        void Start()
        {

        }

        void OnGUI()
        {
            if (GUILayout.Button("Load", GUILayout.Width(80)))
            {
                //Path to aiImporter executable
                string exePath = Application.dataPath + "/aiImporter/Importer/AssimpImporterDemo.exe";

                //Path to model that we want to load
                //Setting it to "FileDialog" will make the aiImporter open a file browser to choose the file
                string filePath = "FileDialog";

                //You can give a file path without opening the file dialog
                //string filePath = "C:\\Users\\User\\Desktop\\test.fbx";

                //Run with the cmd window
                Process aiImporter = Process.Start(exePath, lookFor + " " + "\"" + filePath + "\"");

                /*
				//Use this if you don't want to see the cmd window. Process will run in the background.
				Process process = new Process();
				process.StartInfo.FileName = exePath;
				process.StartInfo.Arguments = lookFor + " " + "\"" + filePath + "\"";           
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.WorkingDirectory = Application.dataPath + "/aiImporter/Importer/";
				process.StartInfo.UseShellExecute=false;
				process.Start();
				*/

                aiImporter.WaitForExit();

                if (aiImporter.ExitCode == 0)
                    LoadFile();
            }
        }


        void LoadFile()
        {
            using (var file = File.OpenRead(Application.dataPath + "/aiImporter/Importer/" + lookFor))
            {
                unityScene = Serializer.Deserialize<GameScene>(file);
            }

            //Don't need the file anymore. Can delete it.
            File.Delete(Application.dataPath + "/aiImporter/Importer/" + lookFor);

            CreateScene();

            CancelInvoke();
        }
			

        void CreateScene()
        {
            string modelName = getModelName(unityScene.filepath);

            mainObject = new GameObject(modelName);

			Model rootNode = unityScene.rootNode;

			CreateObjects(rootNode, null);
        }

		void CreateObjects(Model model, Transform parentT)
		{
			GameObject temp = new GameObject (model.name);

			if (parentT == null)
				temp.transform.parent = mainObject.transform;
			else
				temp.transform.parent = parentT;

			MatrixUtils.SetTransformMatrix (temp.transform, model.transformMatrix);


			foreach (Geometry gMesh in model.geometry)
			{
				GameObject meshHolder = new GameObject("Mesh");

				meshHolder.transform.parent = temp.transform;
				meshHolder.transform.localPosition = new Vector3(0f, 0f, 0f);
				meshHolder.transform.localScale = new Vector3(1f, 1f, 1f);
				meshHolder.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

			
				var meshRenderer = meshHolder.AddComponent<MeshRenderer>();
				var meshFilter = meshHolder.AddComponent<MeshFilter>();

				Mesh mesh = new Mesh();

				mesh.vertices = gMesh.vertices != null ? gMesh.vertices.ConvertAll(item => (Vector3)item).ToArray() : null;

				#if UNITY_5
				switch (gMesh.uvCount) 
				{
				case 4:
					mesh.uv4 = gMesh.uv4.ConvertAll(item => (Vector2)item).ToArray();
					goto case 3;
				case 3:
					mesh.uv3 = gMesh.uv3.ConvertAll(item => (Vector2)item).ToArray();
					goto case 2;
				case 2:
					mesh.uv2 = gMesh.uv2.ConvertAll(item => (Vector2)item).ToArray();
					goto case 1;
				case 1:
					mesh.uv = gMesh.uv != null  ? gMesh.uv.ConvertAll(item => (Vector2)item).ToArray() : null;
					break;
				}
				#else
				switch (gMesh.uvCount)
				{
				case 4:
				case 3:
					mesh.uv2 = gMesh.uv3.ConvertAll(item => (Vector2)item).ToArray();
					goto case 2;
				case 2:
					mesh.uv1 = gMesh.uv2.ConvertAll(item => (Vector2)item).ToArray();
					goto case 1;
				case 1:
					mesh.uv = gMesh.uv != null ? gMesh.uv.ConvertAll(item => (Vector2)item).ToArray() : null;
					break;
				}
				#endif

				mesh.triangles = gMesh.triangles;

				mesh.normals = gMesh.normals != null ? gMesh.normals.ConvertAll(item => (Vector3)item).ToArray() : null;

				//Finally set the mesh to make it visible
				meshFilter.mesh = mesh;
				meshRenderer.material = new Material (Shader.Find ("Diffuse"));
			}

			foreach (Model child in model.children)
				CreateObjects(child, temp.transform);
		}

        #region HelperFunctions
     
        string getModelName(string path)
        {
            path = path.Replace("\\", "/");

            string temp = path.Substring(path.LastIndexOf("/") + 1);
            temp = temp.Substring(0, temp.LastIndexOf("."));

            return temp;
        }

        #endregion
    }
}
