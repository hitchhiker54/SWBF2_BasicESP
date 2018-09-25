using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicESP
{
    class Player
    {
        public String Name { get; private set; }
        public UInt32 TeamId { get; private set; }
        public Frostbite.PoseType Pose { get; private set; }
        public Frostbite.TransformAABBStruct TransformAABB;
        public float Distance { get; set; }                 // set in GameManager
        public Vector3 Position { get; private set; }
        public Vector2 ScreenPosition { get; set; }         // set in GameManager
        public Vector3 Velocity { get; private set; }
        public float Pitch { get; private set; }
        public float Yaw { get; private set; }
        public bool IsVisible { get; private set; }
        public bool IsSprinting { get; private set; }
        public bool InVehicle { get; private set; }
        public float Health { get; private set; }
        public float MaxHealth { get; private set; }
        public float HeadoffsetY { get; private set; }

        public bool IsDead
        {
            get
            {
                return ((Health < 0.1f) || (TeamId == 0));
            }
        }
        public bool IsValidPlayer { get; private set; }

        private Frostbite.WSClientSoldierEntity CSoldier;
        private Frostbite.WSClientVehicleEntity CVehicle;
        private Frostbite.ClientPlayer CPlayer;

        public Player(ulong playerPointer)
        {
            ulong EntityComponents;

            IsValidPlayer = false;
            InVehicle = false;

            if (!RPM.IsValid(playerPointer)) { return; }
            CPlayer = RPM.Read<Frostbite.ClientPlayer>(playerPointer);

            if (CPlayer.x003C != 511) { return; }

            if (!RPM.IsValid(CPlayer.m_ControlledControllable)) { return; }
            CSoldier = RPM.Read<Frostbite.WSClientSoldierEntity>(CPlayer.m_ControlledControllable);

            Name = RPM.ReadString(CPlayer.m_Name, 32);
            TeamId = CPlayer.m_TeamId;

            if (RPM.IsValid(CPlayer.m_AttachedControllable))
            {
                CVehicle = RPM.Read<Frostbite.WSClientVehicleEntity>(CPlayer.m_AttachedControllable);
                InVehicle = true;
            }

            Frostbite.HealthComponent CHealthComponent;

            if (!InVehicle)
            {
                Pose = (Frostbite.PoseType)CSoldier.m_Pose;

                IsVisible = !(CSoldier.m_IsOccluded == 1);
                IsSprinting = (CSoldier.m_IsSprinting == 1);

                Pitch = CSoldier.m_PitchRads;
                Yaw = CSoldier.m_YawRads;

                HeadoffsetY = CSoldier.m_HeadboneOffsetY;

                Velocity = (Vector3)CSoldier.m_Velocity;

                if (!RPM.IsValid(CSoldier.m_HealthComponent)) { return; }
                CHealthComponent = RPM.Read<Frostbite.HealthComponent>(CSoldier.m_HealthComponent);

                TransformAABB.AABB.Min = new Vector4(-0.350000f, 0.000000f, -0.350000f, 0);
                TransformAABB.AABB.Max = new Vector4(0.350000f, (CSoldier.m_HeadboneOffsetY + 0.3f), 0.350000f, 0);

                EntityComponents = CSoldier.m_EntityComponents;
            }
            else
            {
                if (!RPM.IsValid(CVehicle.m_HealthComponent)) { return; }
                CHealthComponent = RPM.Read<Frostbite.HealthComponent>(CVehicle.m_HealthComponent);
                Velocity = (Vector3)CVehicle.m_Velocity;

                TransformAABB.AABB.Min = CVehicle.m_MinAABB;
                TransformAABB.AABB.Max = CVehicle.m_MaxAABB;

                EntityComponents = CVehicle.m_EntityComponents;
            }

            Health = CHealthComponent.m_Health;
            MaxHealth = CHealthComponent.m_MaxHealth;

            if (IsDead) { return; }

            { // huangfengye's function for transformation matrix from any entity
                byte ECX;
                byte EAX;
                ulong pTrans;

                ECX = RPM.Read<byte>(EntityComponents + 0x09);
                EAX = RPM.Read<byte>(EntityComponents + 0x0A);

                pTrans = (ulong)((ECX + EAX * 2) * 0x20);

                var temp = EntityComponents + pTrans + 0x10;

                TransformAABB.Matrix = RPM.Read<Matrix>(EntityComponents + pTrans + 0x10);
            }

            Position = TransformAABB.Matrix.TranslationVector;

            IsValidPlayer = true;
        }
    }
}
