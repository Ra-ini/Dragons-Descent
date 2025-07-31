using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;
using Verse.Sound;
using Verse.Steam;

namespace DD
{
    public class CompAutoGrower : ThingComp
    {
        protected int targetAge = 35;
        protected bool active = false;

        public CompProperties_AutoGrower Props => (CompProperties_AutoGrower)props;

        protected Pawn PawnOwner
        {
            get
            {
                if (parent is Pawn result)
                {
                    return result;
                }
                return null;
            }
        }

        // protected override bool Active
        // {
        //     get
        //     {
        //         if (!base.Active)
        //         {
        //             return false;
        //         }

        //         Pawn pawn = parent as Pawn;
        //         return active && pawn.ageTracker.age < targetAge;
        //     }
        // }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref targetAge, "targetAge", 35);
            Scribe_Values.Look(ref active, "active", false);
        }

        public override void CompTick()
        {
            base.CompTick();

            // Don't try if checkbox is disabled or target age reached
            Pawn pawn = parent as Pawn;
            if (!active) return;
            if (pawn.ageTracker.AgeBiologicalYears >= targetAge)
            {
                active = false;
                return;
            }

            // Let's not pile the bills!
            if (pawn.health.surgeryBills.Bills.Any(bill => bill.recipe.defName == "Administer_MoonStone" || bill.recipe.defName == "Administer_DraconicAmbrosia")) return;

            // Let's not ask for administration while there is a growth hediff
            if (pawn.health.hediffSet.hediffs.Where(hediff => hediff.def.defName == "StrongDraconicDrug" || hediff.def.defName == "WeakDraconicDrug").Any()) return;

            RecipeDef recipe = pawn.def.AllRecipes.FirstOrDefault(recipe => recipe.defName == "Administer_DraconicAmbrosia");
            if (recipe != null && recipe.AvailableNow)
            {
                HealthCardUtility.CreateSurgeryBill(pawn, recipe, null);
                return;
            }
            recipe = pawn.def.AllRecipes.FirstOrDefault(recipe => recipe.defName == "Administer_MoonStone");
            if (recipe != null && recipe.AvailableNow)
            {
                HealthCardUtility.CreateSurgeryBill(pawn, recipe, null);
                return;
            }

            // no option for growth. How sad for little derg.
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo item in base.CompGetGizmosExtra())
            {
                yield return item;
            }
            foreach (Gizmo gizmo in GetGizmos())
            {
                yield return gizmo;
            }
        }

        private IEnumerable<Gizmo> GetGizmos()
        {
            if (PawnOwner.Faction == Faction.OfPlayer && Find.Selector.SingleSelectedThing == PawnOwner && PawnOwner.ageTracker.AgeBiologicalYears < targetAge)
            {
                // Gizmo_AutoGrower gizmo = new Gizmo_AutoGrower();
                // gizmo.status = this;
                // yield return gizmo;
                Command_Toggle command_Toggle = new Command_Toggle();
                command_Toggle.defaultLabel = "Auto Growth";
                command_Toggle.defaultDesc = "Administer available growth drugs automatically.";
                command_Toggle.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/AutoRepair");
                command_Toggle.isActive = () => active;
                command_Toggle.toggleAction = (Action)Delegate.Combine(command_Toggle.toggleAction, (Action)delegate
                {
                    active = !active;
                });
                yield return command_Toggle;
            }
        }
    }
}
