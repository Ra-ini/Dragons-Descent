using System;
using RimWorld;
using Verse;

namespace DD
{
    public class CompProperties_TextureSwapConditional : CompProperties
    {
        public CompProperties_TextureSwapConditional()
        {
            compClass = typeof(CompTextureSwapConditional);
        }

        public GraphicData originalGraphicData;
    }

}