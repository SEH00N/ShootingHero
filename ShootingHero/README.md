### 유니티 프로젝트 폴더 구조
```
00.Scenes
01.Scripts
  ㄴ Editor
      ㄴ ShootingHero.Editor.asmdef
  ㄴ Runtime
    ㄴ Client
      ㄴ ShootingHero.Clients.asmdef
    ㄴ Server
      ㄴ ShootingHero.Servers.asmdef
    ㄴ Shared
      ㄴ ShootingHero.Shared.asmdef
99.ETC
  ㄴ URP
```

```
ShootingHero.Clients.asmdef 에 ShootingHero.Shared.asmdef 참조 추가
ShootingHero.Servers.asmdef 에 ShootingHero.Shared.asmdef 참조 추가

! 필요시 !
ShootingHero.Editor.asmdef 에 ShootingHero.Clients.asmdef 참조 추가
ShootingHero.Editor.asmdef 에 ShootingHero.Servers.asmdef 참조 추가
ShootingHero.Editor.asmdef 에 ShootingHero.Shared.asmdef 참조 추가
```

### 라이브러리 임포트
1. NugetForUnity 추가
2. NugetForUnity - System.Threading.Channels 추가
3. NugerForUnity - MemoryPack 추가
4. Assets/Plugins 하위에 ShootingHeroNetworks.dll 추가