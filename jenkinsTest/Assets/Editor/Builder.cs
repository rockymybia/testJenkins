using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;

namespace PnBuilder
{
    class Builder
    {
        static string[] SCENES = FindEnabledEditorScenes();

        static string APP_NAME = "test";
        static string TARGET_DIR = "Build";

        [MenuItem("Build/AndroidBuild")]
        static void Build()
        {
            string prefix = GetArg( "-outputprefix" );
            string dateTime = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            string target_dir = prefix + "_" + APP_NAME + "_" + dateTime + ".apk";

            //PlayerSettings.keystorePass = "키스토어비번";
            //PlayerSettings.keyaliasPass = "키별칭비번";
            
            string folder = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString( "D2" ) + DateTime.Now.Day.ToString( "D2" );

            if ( Directory.Exists( TARGET_DIR + "/" + folder ) == false )
            {
                Directory.CreateDirectory( TARGET_DIR + "/" + folder );
            }

            GenericBuild( SCENES, TARGET_DIR + "/" + folder + "/" + target_dir, BuildTargetGroup.Android, BuildTarget.Android, BuildOptions.None );
        }

        private static string [] FindEnabledEditorScenes()
        {
            List<string> EditorScenes = new List<string>();
            foreach ( EditorBuildSettingsScene scene in EditorBuildSettings.scenes )
            {
                if ( !scene.enabled )
                    continue;
                EditorScenes.Add( scene.path );
            }
            return EditorScenes.ToArray();
        }

        static void GenericBuild( string [] scenes, string target_dir, BuildTargetGroup build_group, BuildTarget build_target, BuildOptions build_options )
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget( build_group, build_target );
            var res = BuildPipeline.BuildPlayer( scenes, target_dir, build_target, build_options );
            if ( res.summary.result != BuildResult.Succeeded)
            {
                throw new Exception( "BuildPlayer failure: " + res );
            }
        }

        private static string GetArg( string name)
        {
            string[] arguments = System.Environment.GetCommandLineArgs();

            for ( int nIndex = 0; nIndex < arguments.Length; ++nIndex )
            {
                if ( arguments[ nIndex ] == name && arguments.Length > nIndex + 1 )
                {
                    return arguments [nIndex + 1];
                }
            }

            return null;
        }
    }

}