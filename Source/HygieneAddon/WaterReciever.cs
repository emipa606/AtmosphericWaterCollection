using DubsBadHygiene;
using RimWorld;
using Verse;

namespace HygieneAddon;

public class WaterReciever : Building_Butt
{
    private DefModExtension_Hygiene defModExtension;

    private DefModExtension_Hygiene DefModExtension
    {
        get
        {
            if (defModExtension == null)
            {
                defModExtension = def.GetModExtension<DefModExtension_Hygiene>();
            }

            return defModExtension;
        }
    }

    public bool WorkingNow =>
        FlickUtility.WantsToBeOn(this) &&
        (powerComp == null ||
         powerComp.PowerOn) && (!defModExtension.forbidRoof ||
                                !RoofUtility.IsAnyCellUnderRoof(
                                    this));

    public int tick => Find.TickManager.TicksGame;

    public override void Tick()
    {
        base.Tick();
        if (tick % 60 != 0 || DefModExtension.weatherDefCounts.NullOrEmpty() || !WorkingNow)
        {
            return;
        }

        var num = DefModExtension.weatherDefCounts
            .Find(x => x.weatherDef == Map.weatherManager.curWeather)?.water ?? 0f;
        pipe.pipeNet.PushWater(num);
    }
}