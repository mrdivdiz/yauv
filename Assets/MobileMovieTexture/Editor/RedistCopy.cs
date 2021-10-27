using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

namespace MMT
{
    public class RedistCopy
    {
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
		    if (target == BuildTarget.StandaloneWindows) 
            {
                const string RedistPath = "Assets/MobileMovieTexture/Editor/x86Redist";
	
                const string fileName = "pthreadVC2.dll";

                try
                {
                    var projectPath = Path.GetDirectoryName(pathToBuiltProject);
                    File.Copy(Path.Combine(RedistPath, fileName), Path.Combine(projectPath, fileName));
                }
                catch (System.Exception ex)
                {
                    Debug.LogException(ex);
                }
                
		    }		
        }
    }
}