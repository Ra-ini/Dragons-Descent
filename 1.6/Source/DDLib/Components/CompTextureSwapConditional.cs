using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace DD
{

    [StaticConstructorOnStartup]
    public class CompTextureSwapConditional : ThingComp
    {
        public CompProperties_TextureSwapConditional Props => (CompProperties_TextureSwapConditional)props;
        public Pawn pawn => parent as Pawn;

        private bool isActive;

        public bool IsActive { get => isActive; set => isActive = value; }

        public override void CompTick()
        {
            base.CompTick();

            if (isActive != DraconicOverseer.Settings.TextureForceConvertLegacy)
            {
                ForceRedraw(pawn);
                isActive = DraconicOverseer.Settings.TextureForceConvertLegacy;
            }
        }

        public static void ForceRedraw(Pawn pawn)
        {
            pawn.Drawer?.renderer.SetAllGraphicsDirty();
        }
    }
}