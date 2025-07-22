using Verse;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using LudeonTK;
using UnityEngine;

namespace DD
{
    public class PawnRenderNode_Dragon : PawnRenderNode_AnimalPart
    {
        public PawnRenderNode_Dragon(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree) { }

        public override Graphic GraphicFor(Pawn pawn)
        {
            if (pawn.TryGetComp<CompTextureSwapConditional>(out var comp) && comp != null && comp.IsActive && tree.Resolved)
            {
                Graphic graphic = pawn.ageTracker.CurKindLifeStage.bodyGraphicData.Graphic;
                return GraphicDatabase.Get<Graphic_Multi>("Things/Pawn/Animal/BLDragon/BLDragon8", ShaderDatabase.Cutout, graphic.drawSize, Color.white, Color.white, graphic.data);
            }
            else
            {
                return base.GraphicFor(pawn);
            }
        }
    }
}
