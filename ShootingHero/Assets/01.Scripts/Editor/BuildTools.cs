using UnityEditor;

namespace ShootingHero.Editors
{
    public static class BuildTools
    {
        private static readonly BuildPlayerOptions CLIENT_BUILD_OPTIONS = new BuildPlayerOptions() {
            scenes = new string[] {
                "Assets/00.Scenes/Release/ClientBootstrap.unity",
                "Assets/00.Scenes/Release/Lobby.unity",
                "Assets/00.Scenes/Release/Game.unity"
            },
            locationPathName = "Build\\ShootingHero\\ShootingHero.app",
            target = BuildTarget.StandaloneOSX,
            subtarget = (int)StandaloneBuildSubtarget.Player,
            options = BuildOptions.Development
        };

        private static readonly BuildPlayerOptions SERVER_BUILD_OPTIONS = new BuildPlayerOptions() {
            scenes = new string[] {
                "Assets/00.Scenes/Release/ServerBootstrap.unity",
                "Assets/00.Scenes/Release/Game.unity"
            },
            locationPathName = "Build\\ShootingHeroGameServer\\ShootingHeroGameServer",
            target = BuildTarget.StandaloneOSX,
            subtarget = (int)StandaloneBuildSubtarget.Server,
            options = BuildOptions.Development
        };

        [MenuItem("ShootingHero/BuildClient")]
        public static void BuildClient()
        {
            BuildPipeline.BuildPlayer(CLIENT_BUILD_OPTIONS);
        }

        [MenuItem("ShootingHero/BuildServer")]
        public static void BuildServer()
        {
            BuildPipeline.BuildPlayer(SERVER_BUILD_OPTIONS);
        }
    }
}