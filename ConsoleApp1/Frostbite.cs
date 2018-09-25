using SharpDX;
using System.Runtime.InteropServices;

namespace BasicESP
{
    public class Frostbite
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct GameRenderer
        {
            [FieldOffset(1296)]
            public ulong m_GameRenderSettings0; //0x0510

            [FieldOffset(1304)]
            public ulong m_GameRenderSettings1; //0x0518

            [FieldOffset(1312)]
            public ulong m_pCompositionSettings; //0x0520

            [FieldOffset(1336)]
            public ulong m_RenderView; //0x0538
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct RenderView
        {
            [FieldOffset(0x0000)]
            public Matrix m_CameraTransform; //0x0000

            [FieldOffset(0x0100)]
            public Matrix m_CameraTransformCopy; //0x0100

            [FieldOffset(0x0270)]
            public Matrix m_ViewMatrix; //0x0270

            [FieldOffset(0x02B0)]
            public Matrix m_ViewMatrixTranspose; //0x02B0

            [FieldOffset(0x02F0)]
            public Matrix m_ViewMatrixInverse; //0x02F0

            [FieldOffset(0x0330)]
            public Matrix m_ProjectionMatrix; //0x0330

            [FieldOffset(0x0370)]
            public Matrix m_ViewMatrixAtOrigin; //0x0370

            [FieldOffset(0x03B0)]
            public Matrix m_ProjectionTranspose; //0x03B0

            [FieldOffset(0x03F0)]
            public Matrix m_ProjectionInverse; //0x03F0

            [FieldOffset(0x0430)]
            public Matrix m_ViewProjection; //0x0430

            [FieldOffset(0x0470)]
            public Matrix m_ViewProjectionTranspose; //0x0470

            [FieldOffset(0x04B0)]
            public Matrix m_ViewProjectionInverse; //0x04B0
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct ClientGameContext
        {
            [FieldOffset(48)]
            public ulong m_GameTime; //0x0030

            [FieldOffset(56)]
            public ulong m_Level; //0x0038

            [FieldOffset(88)]
            public ulong m_ClientPlayerManager; //0x0058

            [FieldOffset(96)]
            public ulong m_OnlineManager; //0x0060

            [FieldOffset(104)]
            public ulong m_ClientGameView; //0x0068
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct ClientPlayerManager
        {
            [FieldOffset(8)]
            public ulong m_PlayerData; //0x0008

            [FieldOffset(240)]
            public ulong m_PlayerList; //0x00F0

            [FieldOffset(248)]
            public ulong m_PlayerListNoLocalPlayer; //0x00F8

            [FieldOffset(1384)]
            public ulong m_LocalPlayer; //0x0568
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct ClientPlayer
        {
            [FieldOffset(8)]
            public ulong m_PlayerData; //0x0008

            [FieldOffset(24)]
            public ulong m_Name; //0x0018

            [FieldOffset(60)]
            public uint x003C; //0x003C can be used as an assertion for valid player as 511

            [FieldOffset(88)]
            public uint m_TeamId; //0x0058 fb::TeamId

            [FieldOffset(384)]
            public ulong m_ThisClientPlayer; //0x0180

            [FieldOffset(512)]
            public ulong m_AttachedControllable; //0x0200

            [FieldOffset(528)]
            public ulong m_ControlledControllable; //0x0210

            [FieldOffset(552)]
            public ulong m_SomethingCamera; //0x0228

            [FieldOffset(584)]
            public uint m_PlayerIndex; //0x0248

            [FieldOffset(608)]
            public ulong m_ClientPlayerManager; //0x0260

            [FieldOffset(720)]
            public ulong m_DiceShooterClientPlayerExtent; //0x02D0
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct WSClientVehicleEntity
        {
            [FieldOffset(48)]
            public ulong m_VehicleEntityData; //0x0030

            [FieldOffset(56)]
            public ulong m_EntityComponents; //0x0038

            [FieldOffset(104)]
            public ulong m_PrevEntity; //0x0068

            [FieldOffset(112)]
            public ulong m_NextEntity; //0x0070

            [FieldOffset(160)]
            public ulong m_PhysicsComponent; //0x00A0

            [FieldOffset(332)]
            public uint m_TeamID; //0x016C

            [FieldOffset(616)]
            public ulong m_HealthComponent; //0x0288

            [FieldOffset(992)]
            public Vector4 m_MinAABB; //0x04000

            [FieldOffset(1008)]
            public Vector4 m_MaxAABB; //0x0410

            [FieldOffset(1056)]
            public Vector4 m_Velocity; //0x0440      // needs testing but included anyway
        }

        //[StructLayout(LayoutKind.Explicit)]
        //public struct WSClientSoldierEntity
        //{
        //    [FieldOffset(24)]
        //    public uint m_RenderFlags; //0x0018

        //    [FieldOffset(40)]
        //    public ulong m_EntityBus; //0x0028

        //    [FieldOffset(48)]
        //    public ulong m_SoldierEntityData; //0x0030

        //    [FieldOffset(56)]
        //    public ulong m_EntityComponents; //0x0038

        //    [FieldOffset(104)]
        //    public ulong m_PrevEntity; //0x0068 Prev of same entity type in list

        //    [FieldOffset(112)]
        //    public ulong m_NextEntity; //0x0070 Next "" "" 

        //    [FieldOffset(160)]
        //    public ulong m_PhysicsComponent; //0x00A0

        //    [FieldOffset(332)]
        //    public uint m_TeamID; //0x014C

        //    [FieldOffset(360)]
        //    public ulong m_ClientCharacterSpawnEntity; //0x0168

        //    [FieldOffset(440)]
        //    public ulong m_ThisSoldier; //0x01B8

        //    [FieldOffset(488)]
        //    public ulong m_ppClientPlayerEntryComponent; //0x01E8

        //    [FieldOffset(520)]
        //    public ulong m_ThisPlayer; //0x0208

        //    [FieldOffset(616)]
        //    public ulong m_HealthComponent; //0x0268

        //    [FieldOffset(728)]
        //    public ulong m_SoldierBlueprint; //0x02D8

        //    [FieldOffset(760)]
        //    public ulong m_ClientAntAnimatableEntity1; //0x02F8

        //    [FieldOffset(808)]
        //    public ulong m_ToAnimationControl; //0x0328

        //    [FieldOffset(944)]
        //    public ulong m_ClientAntAnimatableEntity1b; //0x03B0

        //    [FieldOffset(952)]
        //    public ulong m_ClientAntAnimatableEntity2; //0x03B8

        //    [FieldOffset(1024)]
        //    public ulong m_ClientSoldierCameraComponent; //0x0400

        //    [FieldOffset(1136)]
        //    public float m_HeadboneOffsetZ; //0x0470

        //    [FieldOffset(1140)]
        //    public float m_HeadboneOffsetY; //0x0474 static offsets from m_Position

        //    [FieldOffset(1144)]
        //    public float m_HeadboneOffsetX; //0x0478

        //    [FieldOffset(1192)]
        //    public ulong m_SpottingTargetComponentData; //0x04A8

        //    [FieldOffset(1280)]
        //    public float unk_f0; //0x0500 rotate on z?

        //    [FieldOffset(1288)]
        //    public float unk_f1; //0x0508 change when char rotates Y

        //    [FieldOffset(1296)]
        //    public float unk_f2; //0x0510 corresponds to inverse matrix transaltion z

        //    [FieldOffset(1384)]
        //    public float m_StrafeDirection; //0x0568

        //    [FieldOffset(1408)]
        //    public Vector4 m_Velocity; //0x0580

        //    [FieldOffset(1440)]
        //    public ulong m_ClientBoneCollisionComponentPlus0x48; //0x05A0

        //    [FieldOffset(1776)]
        //    public ulong m_SomeOtherReplication; //0x06F0

        //    [FieldOffset(1784)]
        //    public ulong m_ClientSoldierReplication; //0x06F8

        //    [FieldOffset(1812)]
        //    public float m_YawRads; //0x0714 matches local aimer yaw

        //    [FieldOffset(2192)]
        //    public Vector4 m_Shootspace; //0x0890

        //    [FieldOffset(2336)]
        //    public float m_Pitch__; //0x0920

        //    [FieldOffset(2340)]
        //    public float m_Yaw__; //0x0924 almost yaw/pitch from aimer but a little off

        //    [FieldOffset(2372)]
        //    public float m_PitchRads; //0x0944 matches local aimer pitch

        //    [FieldOffset(2376)]
        //    public uint m_Pose; //0x0948

        //    [FieldOffset(2380)]
        //    public byte m_LocalRenderFlags; //0x094C 

        //    [FieldOffset(2464)]
        //    public ulong m_ClientSoldierWeaponsComponent; //0x09A0

        //    [FieldOffset(2472)]
        //    public ulong m_ClientSoldierBodyComponent; //0x09A8

        //    [FieldOffset(2488)]
        //    public ulong m_ClientVehicleEntryListenerComponent; //0x09B8

        //    [FieldOffset(2496)]
        //    public ulong m_ClientAimEntity; //0x09C0

        //    [FieldOffset(2504)]
        //    public ulong m_ClientSoldierWeapon; //0x09C8

        //    [FieldOffset(2536)]
        //    public byte m_IsSprinting; //0x09E8

        //    [FieldOffset(2552)]
        //    public byte m_IsOccluded; //0x09F8

        //    [FieldOffset(2792)]
        //    public ulong m_WSSoldierCustomizationGameplayAsset; //0x0AE8

        //    [FieldOffset(2800)]
        //    public ulong m_WSSoldierCustomizationKitAsset; //0x0AF0
        //}

        [StructLayout(LayoutKind.Explicit)]
        public struct WSClientSoldierEntity // needs full redo
        {
            [FieldOffset(0)]
            public ulong vtable; //0x0000

            [FieldOffset(8)]
            public ulong m_wpBoolEntity; //0x0008 EntityBusPeer end 0x0008

            [FieldOffset(16)]
            public ulong m_wpThisSoldier; //0x0010

            [FieldOffset(24)]
            public uint m_RenderFlags; //0x0018

            [FieldOffset(28)]
            public uint x001C; //0x001C

            [FieldOffset(32)]
            public ushort x0020; //0x0020 Entity end 0x0020

            [FieldOffset(34)]
            public byte x0022; //0x0022

            [FieldOffset(40)]
            public ulong m_EntityBus; //0x0028 SpatialEntity end 0x0028

            [FieldOffset(48)]
            public ulong m_SoldierEntityData; //0x0030

            [FieldOffset(56)]
            public ulong m_EntityComponents; //0x0038

            [FieldOffset(96)]
            public ulong funcTbl_64_x0060; //0x0060 ComponentEntity end 0x0060

            [FieldOffset(104)]
            public ulong m_PrevEntity; //0x0068 Prev of same entity type in list (entity +0x68)

            [FieldOffset(112)]
            public ulong m_NextEntity; //0x0070 Next "" "" or pointer to list start in typeinfo area

            [FieldOffset(120)]
            public uint x0078; //0x0078

            [FieldOffset(124)]
            public uint x007C; //0x007C

            [FieldOffset(128)]
            public uint x0080; //0x0080

            [FieldOffset(132)]
            public uint x0084; //0x0084

            [FieldOffset(136)]
            public byte x0088; //0x0088

            [FieldOffset(138)]
            public byte x008A; //0x008A

            [FieldOffset(140)]
            public int x008C; //0x008C

            [FieldOffset(192)]
            public ulong m_PhysicsComponent; //0x00C0

            [FieldOffset(336)]
            public uint x0130; //0x0150

            [FieldOffset(344)]
            public ulong m_UnkClass0x138; //0x0158

            [FieldOffset(352)]
            public ulong funcTbl_5_x0140; //0x0160

            [FieldOffset(364)]
            public uint m_TeamID; //0x016C

            [FieldOffset(392)]
            public ulong m_ClientCharacterSpawnEntity; //0x0188

            [FieldOffset(408)]
            public ulong m_wpThisSoldier1; //0x0198

            [FieldOffset(416)]
            public ulong funcTbl_unk_x0180; //0x01A0

            [FieldOffset(424)]
            public ulong funcTbl_unk_x0188; //0x01A8

            [FieldOffset(464)]
            public ulong funcTbl_unk_x01B0; //0x01D0

            [FieldOffset(472)]
            public ulong m_ThisSoldier; //0x01D8

            [FieldOffset(520)]
            public ulong m_ppClientPlayerEntryComponent; //0x0208

            [FieldOffset(528)]
            public ulong m_ppThisEntity0; //0x0210

            [FieldOffset(536)]
            public ulong m_ppThisEntity1; //0x0218

            [FieldOffset(544)]
            public ulong x0200; //0x0220

            [FieldOffset(552)]
            public ulong m_ThisPlayer; //0x0228

            [FieldOffset(624)]
            public byte x0250; //0x0270

            [FieldOffset(632)]
            public uint x258; //0x0278

            [FieldOffset(648)]
            public ulong m_HealthComponent; //0x0288

            [FieldOffset(656)]
            public uint x0270; //0x0290

            [FieldOffset(660)]
            public byte x0274; //0x0294

            [FieldOffset(664)]
            public uint x0278; //0x0298

            [FieldOffset(668)]
            public uint x027C; //0x029C

            [FieldOffset(704)]
            public uint x02A0; //0x02C0

            [FieldOffset(720)]
            public ulong funcTbl_15_x02B0; //0x02D0

            [FieldOffset(752)]
            public ulong m_ThisSoldier1; //0x02F0

            [FieldOffset(760)]
            public ulong m_SoldierBlueprint; //0x02F8

            [FieldOffset(769)]
            public byte x02E1; //0x0301 also referenced as WORD size

            [FieldOffset(770)]
            public byte x02E2; //0x0302

            [FieldOffset(776)]
            public ulong funcTbl_11_x02E8; //0x0308

            [FieldOffset(792)]
            public ulong m_ClientAntAnimatableEntity1; //0x0318

            [FieldOffset(824)]
            public ulong funcTbl_32_x0318; //0x0338

            [FieldOffset(832)]
            public ulong funcTbl_31_x0320; //0x0340

            [FieldOffset(840)]
            public ulong m_ToAnimationControl; //0x0348

            [FieldOffset(872)]
            public ulong funcTbl_28_x0348; //0x0368

            [FieldOffset(880)]
            public ulong m_ThisSoldier2; //0x0370

            [FieldOffset(888)]
            public ulong funcTbl_27_x0358; //0x0378

            [FieldOffset(976)]
            public ulong m_ClientAntAnimatableEntity1b; //0x03D0

            [FieldOffset(984)]
            public ulong m_ClientAntAnimatableEntity2; //0x03D8

            [FieldOffset(992)]
            public uint x03C0; //0x03E0

            [FieldOffset(1000)]
            public uint x03C8; //0x03E8 a model index or typeinfo ref

            [FieldOffset(1008)]
            public ushort x03D0; //0x03F0

            [FieldOffset(1012)]
            public ushort x03D4; //0x03F4

            [FieldOffset(1016)]
            public ushort x03D8; //0x03F8

            [FieldOffset(1018)]
            public byte x03DA; //0x03FA

            [FieldOffset(1032)]
            public byte x03E8; //0x0408

            [FieldOffset(1048)]
            public ushort x03F8; //0x0418

            [FieldOffset(1050)]
            public byte x03FA; //0x041A

            [FieldOffset(1052)]
            public uint x03FC; //0x041C

            [FieldOffset(1056)]
            public ulong m_CameraComponent; //0x0420

            [FieldOffset(1072)]
            public uint x0410; //0x0430

            [FieldOffset(1076)]
            public byte x0414; //0x0434

            [FieldOffset(1080)]
            public uint x0418; //0x0438

            [FieldOffset(1088)]
            public ulong funcTbl_71_x0420; //0x0440 ClientCharacterEntity end 0x0420

            [FieldOffset(1104)]
            public ulong m_EntityData2; //0x0450

            [FieldOffset(1120)]
            public byte x0440; //0x0460

            [FieldOffset(1140)]
            public uint x0454; //0x0474

            [FieldOffset(1168)]
            public float m_HeadboneOffsetZ; //0x0490

            [FieldOffset(1172)]
            public float m_HeadboneOffsetY; //0x0494 static offsets from m_Position

            [FieldOffset(1176)]
            public float m_HeadboneOffsetX; //0x0498

            [FieldOffset(1312)]
            public float unk_f0; //0x0520 something referenced in occlusion calc?

            [FieldOffset(1320)]
            public float unk_f1; //0x0528 change when char rotates Y

            [FieldOffset(1328)]
            public float unk_f2; //0x0530 corresponds to inverse matrix transaltion z

            [FieldOffset(1416)]
            public float m_StrafeDirection; //0x0588

            [FieldOffset(0x05A0)]
            public Vector4 m_Velocity; //0x05A0

            [FieldOffset(1472)]
            public ulong m_ClientBoneCollisionComponentPlus0x48; //0x05C0

            [FieldOffset(1496)]
            public float x05B8; //0x05D8

            [FieldOffset(1500)]
            public float x05Bc; //0x05DC

            [FieldOffset(1504)]
            public byte x05C0; //0x05E0

            [FieldOffset(1520)]
            public ulong funcTbl_47_x05D0; //0x05F0

            [FieldOffset(1600)]
            public ulong x0620; //0x0640

            [FieldOffset(1608)]
            public ushort x0628; //0x0648

            [FieldOffset(1610)]
            public byte x062A; //0x064A

            [FieldOffset(1616)]
            public ulong funcTbl_13_x0630; //0x0650

            [FieldOffset(1624)]
            public ulong funcTbl_43_x0638; //0x0658

            [FieldOffset(1632)]
            public ulong funcTbl_unk_x0640; //0x0660

            [FieldOffset(1640)]
            public ulong funcTbl_unk_x0648; //0x0668

            [FieldOffset(1680)]
            public ulong funcTbl_unk_x0670; //0x0690

            [FieldOffset(1688)]
            public ulong m_ThisSoldier3; //0x0698

            [FieldOffset(1808)]
            public ulong m_SomeOtherReplication; //0x0710

            [FieldOffset(1816)]
            public ulong m_ClientSoldierReplication; //0x0718

            [FieldOffset(1832)]
            public short x0708; //0x0728

            [FieldOffset(1834)]
            public byte x070A; //0x072A

            [FieldOffset(1836)]
            public float x070C; //0x072C

            [FieldOffset(1844)]
            public float m_YawRads; //0x0734 matches local aimer yaw

            [FieldOffset(1848)]
            public byte x0718; //0x0738

            [FieldOffset(1849)]
            public ushort x0719; //0x0739

            [FieldOffset(1851)]
            public byte x071B; //0x073B

            [FieldOffset(1852)]
            public uint x071C; //0x073C

            [FieldOffset(1856)]
            public ulong funcTbl_unk_x0720; //0x0740

            [FieldOffset(1864)]
            public ulong funcTbl_unk_x0728; //0x0748

            [FieldOffset(1952)]
            public ulong funcTbl_unk_x0780; //0x07A0

            [FieldOffset(2008)]
            public ulong funcTbl_unk_x07B8; //0x07D8

            [FieldOffset(2088)]
            public uint x0808; //0x0828

            [FieldOffset(2096)]
            public ulong m_ThisSoldier4; //0x0830

            [FieldOffset(2368)]
            public float m_Pitch__; //0x0940

            [FieldOffset(2372)]
            public float m_Yaw__; //0x0944 almost yaw/pitch from aimer but a little off

            [FieldOffset(2376)]
            public float m_Pitch; //0x0948

            [FieldOffset(2380)]
            public float m_Yaw; //0x094C

            [FieldOffset(2384)]
            public byte x0930; //0x0950

            [FieldOffset(2388)]
            public float x0934; //0x0954

            [FieldOffset(2396)]
            public byte x093C; //0x095C

            [FieldOffset(2397)]
            public byte x093D; //0x095D

            [FieldOffset(2398)]
            public byte x093E; //0x095E

            [FieldOffset(2399)]
            public byte x093F; //0x095F

            [FieldOffset(2400)]
            public byte x0940; //0x0960

            [FieldOffset(2404)]
            public float m_PitchRads; //0x0964 matches local aimer pitch

            [FieldOffset(2408)]
            public uint m_Pose; //0x0968

            [FieldOffset(2412)]
            public byte m_LocalRenderFlags; //0x096C local (self) render flags - bit 1 set = invisible id localplayer

            [FieldOffset(2413)]
            public byte x094D; //0x096D

            [FieldOffset(2415)]
            public byte x094F; //0x096F

            [FieldOffset(2416)]
            public uint x0950; //0x0970

            [FieldOffset(2452)]
            public uint x0974; //0x0994

            [FieldOffset(2488)]
            public byte x0998; //0x09B8

            [FieldOffset(2492)]
            public float x099C; //0x09BC

            [FieldOffset(2496)]
            public ulong m_ClientSoldierWeaponsComponent; //0x09C0

            [FieldOffset(2504)]
            public ulong m_ClientSoldierBodyComponent; //0x09C8

            [FieldOffset(2520)]
            public ulong m_ClientVehicleEntryListenerComponent; //0x09D8

            [FieldOffset(2528)]
            public ulong m_ClientAimEntity; //0x09E0

            [FieldOffset(2536)]
            public ulong m_ClientSoldierWeapon; //0x09E8

            [FieldOffset(2544)]
            public ushort x09D0; //0x09F0

            [FieldOffset(2546)]
            public byte x09D2; //0x09F2

            [FieldOffset(2560)]
            public uint x09E0; //0x0A00

            [FieldOffset(2564)]
            public float x09E4; //0x0A04

            [FieldOffset(2568)]
            public byte m_IsSprinting; //0x0A08

            [FieldOffset(2576)]
            public ulong funcTbl_unk_x09F0; //0x0A10

            [FieldOffset(0x0A18)]
            public byte m_IsOccluded; //0x0A18

            [FieldOffset(2585)]
            public byte x09F9; //0x0A19

            [FieldOffset(2624)]
            public byte x0A20; //0x0A40

            [FieldOffset(2688)]
            public ulong m_ClientVoiceOverObjectReaderEntity; //0x0A80

            [FieldOffset(2696)]
            public uint x0A68; //0x0A88

            [FieldOffset(2700)]
            public ushort x0A6C; //0x0A8C

            [FieldOffset(2702)]
            public byte x0A6E; //0x0A8E

            [FieldOffset(2704)]
            public float x0A70; //0x0A90

            [FieldOffset(2708)]
            public uint x0A74; //0x0A94

            [FieldOffset(2712)]
            public long x0A78; //0x0A98

            [FieldOffset(2720)]
            public ulong m_ThisSoldier5; //0x0AA0

            [FieldOffset(2752)]
            public byte x0AA0; //0x0AC0

            [FieldOffset(2756)]
            public float x0AA4; //0x0AC4

            [FieldOffset(2760)]
            public uint x0AA8; //0x0AC8

            [FieldOffset(2764)]
            public byte x0AAC; //0x0ACC

            [FieldOffset(2765)]
            public byte x0AAD; //0x0ACD

            [FieldOffset(2784)]
            public byte x0AC0; //0x0AE0

            [FieldOffset(2816)]
            public ulong funcPtr0x0AE0; //0x0B00 ClientSoldierEntity end 0X0AE0

            [FieldOffset(2824)]
            public ulong m_WSSoldierCustomizationGameplayAsset; //0x0B08

            [FieldOffset(2832)]
            public ulong m_WSSoldierCustomizationKitAsset; //0x0B10
        }


        [StructLayout(LayoutKind.Explicit)]
        public struct HealthComponent
        {
            [FieldOffset(32)]
            public float m_Health; //0x0020

            [FieldOffset(36)]
            public float m_MaxHealth; //0x0024
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct ClientSoldierReplication
        {
            [FieldOffset(32)]
            public Vector4 m_Position; //0x0020

            [FieldOffset(48)]
            public Vector4 m_Velocity; //0x0030
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct AxisAlignedBox
        {
            [FieldOffset(0)]
            public Vector4 Min;  //0x0000

            [FieldOffset(16)]
            public Vector4 Max;  //0x0010
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct TransformAABBStruct
        {
            [FieldOffset(0)]
            public Matrix Matrix;

            [FieldOffset(0x0040)]
            public AxisAlignedBox AABB;
        }

        public enum PoseType
        {
            Standing = 0,
            Crouching = 1,
            Prone = 2
        }
    }
}
