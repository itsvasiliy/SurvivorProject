using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DistantLands.Cozy
{
    public static class CozyShaderIDs
    {
        public static int CZY_FogColor1ID;
        public static int CZY_FogColor2ID;
        public static int CZY_FogColor3ID;
        public static int CZY_FogColor4ID;
        public static int CZY_FogColor5ID;
        public static int CZY_FogLitColorID;
        public static int CZY_FogShadowColorID;
        public static int CZY_FogColorStart1ID;
        public static int CZY_FogColorStart2ID;
        public static int CZY_FogColorStart3ID;
        public static int CZY_FogColorStart4ID;
        public static int CZY_FogIntensityID;
        public static int CZY_FogOffsetID;
        public static int CZY_LightFlareSquishID;
        public static int CZY_FogSmoothnessID;
        public static int CZY_FogDepthMultiplierID;
        public static int CZY_LightColorID;
        public static int CZY_FogMoonFlareColorID;
        public static int CZY_VariationAmountID;
        public static int CZY_VariationScaleID;
        public static int CZY_VariationWindDirectionID;
        public static int CZY_VariationDistanceID;
        public static int CZY_SunDirectionID;
        public static int CZY_LightFalloffID;
        public static int CZY_LightIntensityID;
        public static int CZY_FilterColorID;
        public static int CZY_SunFilterColorID;
        public static int CZY_CloudFilterColorID;
        public static int CZY_FilterValueID;
        public static int CZY_FilterSaturationID;
        public static int CZY_CumulusCoverageMultiplierID;
        public static int CZY_NimbusMultiplierID;
        public static int CZY_NimbusHeightID;
        public static int CZY_NimbusVariationID;
        public static int CZY_BorderHeightID;
        public static int CZY_BorderEffectID;
        public static int CZY_BorderVariationID;
        public static int CZY_AltocumulusMultiplierID;
        public static int CZY_CirrostratusMultiplierID;
        public static int CZY_ChemtrailsMultiplierID;
        public static int CZY_CirrusMultiplierID;
        public static int CZY_CloudTextureID;
        public static int CZY_ChemtrailsTextureID;
        public static int CZY_CirrusTextureID;
        public static int CZY_CirrostratusTextureID;
        public static int CZY_AltocumulusTextureID;
        public static int CZY_PartlyCloudyLuxuryCloudsTextureID;
        public static int CZY_MostlyCloudyLuxuryCloudsTextureID;
        public static int CZY_OvercastLuxuryCloudsTextureID;
        public static int CZY_LowBorderLuxuryCloudsTextureID;
        public static int CZY_HighBorderLuxuryCloudsTextureID;
        public static int CZY_LowNimbusLuxuryCloudsTextureID;
        public static int CZY_MidNimbusLuxuryCloudsTextureID;
        public static int CZY_HighNimbusLuxuryCloudsTextureID;
        public static int CZY_LuxuryVariationTextureID;
        public static int CZY_StarMapID;
        public static int CZY_GalaxyStarMapID;
        public static int CZY_GalaxyVariationMapID;
        public static int CZY_LightScatteringMapID;
        public static int CZY_GalaxyMapID;
        public static int CZY_TexturePanDirectionID;
        public static int CZY_ZenithColorID;
        public static int CZY_HorizonColorID;
        public static int CZY_StarColorID;
        public static int CZY_GalaxyMultiplierID;
        public static int CZY_RainbowIntensityID;
        public static int CZY_PowerID;
        public static int CZY_SunSizeID;
        public static int CZY_SunColorID;
        public static int CZY_MoonColorID;
        public static int CZY_SunHaloFalloffID;
        public static int CZY_SunHaloColorID;
        public static int CZY_MoonFlareColorID;
        public static int CZY_MoonFlareFalloffID;
        public static int CZY_GalaxyColor1ID;
        public static int CZY_GalaxyColor2ID;
        public static int CZY_GalaxyColor3ID;
        public static int CZY_LightColumnColorID;
        public static int CZY_RainbowSizeID;
        public static int CZY_RainbowWidthID;
        public static int CZY_StormDirectionID;
        public static int CZY_CloudColorID;
        public static int CZY_CloudHighlightColorID;
        public static int CZY_AltoCloudColorID;
        public static int CZY_CloudTextureColorID;
        public static int CZY_CloudMoonColorID;
        public static int CZY_SunFlareFalloffID;
        public static int CZY_CloudMoonFalloffID;
        public static int CZY_WindSpeedID;
        public static int CZY_CloudCohesionID;
        public static int CZY_SpherizeID;
        public static int CZY_ShadowingDistanceID;
        public static int CZY_ClippingThresholdID;
        public static int CZY_CloudThicknessID;
        public static int CZY_MainCloudScaleID;
        public static int CZY_DetailScaleID;
        public static int CZY_DetailAmountID;
        public static int CZY_TextureAmountID;
        public static int CZY_AltocumulusScaleID;
        public static int CZY_CirrostratusMoveSpeedID;
        public static int CZY_CirrusMoveSpeedID;
        public static int CZY_ChemtrailsMoveSpeedID;
        public static int CZY_DayPercentageID;
        public static int CZY_EclipseDirectionID;
        public static int CZY_MoonSizeID;
        public static int CZY_HeightFogBaseID;
        public static int CZY_HeightFogBaseVariationScaleID;
        public static int CZY_HeightFogBaseVariationAmountID;
        public static int CZY_HeightFogTransitionID;
        public static int CZY_HeightFogDistanceID;
        public static int CZY_HeightFogColorID;
        public static int CZY_HeightFogIntensityID;

        public static void GrabShaderIDs()
        {
            CZY_FogColor1ID = Shader.PropertyToID("CZY_FogColor1");
            CZY_FogColor2ID = Shader.PropertyToID("CZY_FogColor2");
            CZY_FogColor3ID = Shader.PropertyToID("CZY_FogColor3");
            CZY_FogColor4ID = Shader.PropertyToID("CZY_FogColor4");
            CZY_FogColor5ID = Shader.PropertyToID("CZY_FogColor5");
            CZY_FogColorStart1ID = Shader.PropertyToID("CZY_FogColorStart1");
            CZY_FogColorStart2ID = Shader.PropertyToID("CZY_FogColorStart2");
            CZY_FogColorStart3ID = Shader.PropertyToID("CZY_FogColorStart3");
            CZY_FogColorStart4ID = Shader.PropertyToID("CZY_FogColorStart4");
            CZY_FogIntensityID = Shader.PropertyToID("CZY_FogIntensity");
            CZY_FogOffsetID = Shader.PropertyToID("CZY_FogOffset");
            CZY_LightFlareSquishID = Shader.PropertyToID("CZY_LightFlareSquish");
            CZY_FogSmoothnessID = Shader.PropertyToID("CZY_FogSmoothness");
            CZY_FogDepthMultiplierID = Shader.PropertyToID("CZY_FogDepthMultiplier");
            CZY_LightColorID = Shader.PropertyToID("CZY_LightColor");
            CZY_FogMoonFlareColorID = Shader.PropertyToID("CZY_FogMoonFlareColor");
            CZY_VariationAmountID = Shader.PropertyToID("CZY_VariationAmount");
            CZY_VariationScaleID = Shader.PropertyToID("CZY_VariationScale");
            CZY_VariationWindDirectionID = Shader.PropertyToID("CZY_VariationWindDirection");
            CZY_VariationDistanceID = Shader.PropertyToID("CZY_VariationDistance");
            CZY_SunDirectionID = Shader.PropertyToID("CZY_SunDirection");
            CZY_LightFalloffID = Shader.PropertyToID("CZY_LightFalloff");
            CZY_LightIntensityID = Shader.PropertyToID("CZY_LightIntensity");
            CZY_FilterColorID = Shader.PropertyToID("CZY_FilterColor");
            CZY_SunFilterColorID = Shader.PropertyToID("CZY_SunFilterColor");
            CZY_CloudFilterColorID = Shader.PropertyToID("CZY_CloudFilterColor");
            CZY_FilterValueID = Shader.PropertyToID("CZY_FilterValue");
            CZY_FilterSaturationID = Shader.PropertyToID("CZY_FilterSaturation");
            CZY_CumulusCoverageMultiplierID = Shader.PropertyToID("CZY_CumulusCoverageMultiplier");
            CZY_NimbusMultiplierID = Shader.PropertyToID("CZY_NimbusMultiplier");
            CZY_NimbusHeightID = Shader.PropertyToID("CZY_NimbusHeight");
            CZY_NimbusVariationID = Shader.PropertyToID("CZY_NimbusVariation");
            CZY_BorderHeightID = Shader.PropertyToID("CZY_BorderHeight");
            CZY_BorderEffectID = Shader.PropertyToID("CZY_BorderEffect");
            CZY_BorderVariationID = Shader.PropertyToID("CZY_BorderVariation");
            CZY_AltocumulusMultiplierID = Shader.PropertyToID("CZY_AltocumulusMultiplier");
            CZY_CirrostratusMultiplierID = Shader.PropertyToID("CZY_CirrostratusMultiplier");
            CZY_ChemtrailsMultiplierID = Shader.PropertyToID("CZY_ChemtrailsMultiplier");
            CZY_CirrusMultiplierID = Shader.PropertyToID("CZY_CirrusMultiplier");
            CZY_CloudTextureID = Shader.PropertyToID("CZY_CloudTexture");
            CZY_ChemtrailsTextureID = Shader.PropertyToID("CZY_ChemtrailsTexture");
            CZY_CirrusTextureID = Shader.PropertyToID("CZY_CirrusTexture");
            CZY_CirrostratusTextureID = Shader.PropertyToID("CZY_CirrostratusTexture");
            CZY_AltocumulusTextureID = Shader.PropertyToID("CZY_AltocumulusTexture");
            CZY_StarMapID = Shader.PropertyToID("CZY_StarMap");
            CZY_GalaxyStarMapID = Shader.PropertyToID("CZY_GalaxyStarMap");
            CZY_GalaxyVariationMapID = Shader.PropertyToID("CZY_GalaxyVariationMap");
            CZY_LightScatteringMapID = Shader.PropertyToID("CZY_LightScatteringMap");
            CZY_GalaxyMapID = Shader.PropertyToID("CZY_GalaxyMap");
            CZY_TexturePanDirectionID = Shader.PropertyToID("CZY_TexturePanDirection");
            CZY_ZenithColorID = Shader.PropertyToID("CZY_ZenithColor");
            CZY_HorizonColorID = Shader.PropertyToID("CZY_HorizonColor");
            CZY_StarColorID = Shader.PropertyToID("CZY_StarColor");
            CZY_GalaxyMultiplierID = Shader.PropertyToID("CZY_GalaxyMultiplier");
            CZY_RainbowIntensityID = Shader.PropertyToID("CZY_RainbowIntensity");
            CZY_PowerID = Shader.PropertyToID("CZY_Power");
            CZY_SunSizeID = Shader.PropertyToID("CZY_SunSize");
            CZY_SunColorID = Shader.PropertyToID("CZY_SunColor");
            CZY_MoonColorID = Shader.PropertyToID("CZY_MoonColor");
            CZY_SunHaloFalloffID = Shader.PropertyToID("CZY_SunHaloFalloff");
            CZY_SunHaloColorID = Shader.PropertyToID("CZY_SunHaloColor");
            CZY_MoonFlareColorID = Shader.PropertyToID("CZY_MoonFlareColor");
            CZY_MoonFlareFalloffID = Shader.PropertyToID("CZY_MoonFlareFalloff");
            CZY_GalaxyColor1ID = Shader.PropertyToID("CZY_GalaxyColor1");
            CZY_GalaxyColor2ID = Shader.PropertyToID("CZY_GalaxyColor2");
            CZY_GalaxyColor3ID = Shader.PropertyToID("CZY_GalaxyColor3");
            CZY_LightColumnColorID = Shader.PropertyToID("CZY_LightColumnColor");
            CZY_RainbowSizeID = Shader.PropertyToID("CZY_RainbowSize");
            CZY_RainbowWidthID = Shader.PropertyToID("CZY_RainbowWidth");
            CZY_StormDirectionID = Shader.PropertyToID("CZY_StormDirection");
            CZY_CloudColorID = Shader.PropertyToID("CZY_CloudColor");
            CZY_CloudHighlightColorID = Shader.PropertyToID("CZY_CloudHighlightColor");
            CZY_AltoCloudColorID = Shader.PropertyToID("CZY_AltoCloudColor");
            CZY_CloudTextureColorID = Shader.PropertyToID("CZY_CloudTextureColor");
            CZY_CloudMoonColorID = Shader.PropertyToID("CZY_CloudMoonColor");
            CZY_SunFlareFalloffID = Shader.PropertyToID("CZY_SunFlareFalloff");
            CZY_CloudMoonFalloffID = Shader.PropertyToID("CZY_CloudMoonFalloff");
            CZY_WindSpeedID = Shader.PropertyToID("CZY_WindSpeed");
            CZY_CloudCohesionID = Shader.PropertyToID("CZY_CloudCohesion");
            CZY_SpherizeID = Shader.PropertyToID("CZY_Spherize");
            CZY_ShadowingDistanceID = Shader.PropertyToID("CZY_ShadowingDistance");
            CZY_ClippingThresholdID = Shader.PropertyToID("CZY_ClippingThreshold");
            CZY_CloudThicknessID = Shader.PropertyToID("CZY_CloudThickness");
            CZY_MainCloudScaleID = Shader.PropertyToID("CZY_MainCloudScale");
            CZY_DetailScaleID = Shader.PropertyToID("CZY_DetailScale");
            CZY_DetailAmountID = Shader.PropertyToID("CZY_DetailAmount");
            CZY_TextureAmountID = Shader.PropertyToID("CZY_TextureAmount");
            CZY_AltocumulusScaleID = Shader.PropertyToID("CZY_AltocumulusScale");
            CZY_CirrostratusMoveSpeedID = Shader.PropertyToID("CZY_CirrostratusMoveSpeed");
            CZY_CirrusMoveSpeedID = Shader.PropertyToID("CZY_CirrusMoveSpeed");
            CZY_ChemtrailsMoveSpeedID = Shader.PropertyToID("CZY_ChemtrailsMoveSpeed");
            CZY_DayPercentageID = Shader.PropertyToID("CZY_DayPercentage");
            CZY_EclipseDirectionID = Shader.PropertyToID("CZY_EclipseDirection");
            CZY_MoonSizeID = Shader.PropertyToID("CZY_MoonSize");

            CZY_PartlyCloudyLuxuryCloudsTextureID = Shader.PropertyToID("CZY_PartlyCloudyTexture");
            CZY_MostlyCloudyLuxuryCloudsTextureID = Shader.PropertyToID("CZY_MostlyCloudyTexture");
            CZY_OvercastLuxuryCloudsTextureID = Shader.PropertyToID("CZY_OvercastTexture");
            CZY_LowBorderLuxuryCloudsTextureID = Shader.PropertyToID("CZY_LowBorderTexture");
            CZY_HighBorderLuxuryCloudsTextureID = Shader.PropertyToID("CZY_HighBorderTexture");
            CZY_LowNimbusLuxuryCloudsTextureID = Shader.PropertyToID("CZY_LowNimbusTexture");
            CZY_MidNimbusLuxuryCloudsTextureID = Shader.PropertyToID("CZY_MidNimbusTexture");
            CZY_HighNimbusLuxuryCloudsTextureID = Shader.PropertyToID("CZY_HighNimbusTexture");
            CZY_LuxuryVariationTextureID = Shader.PropertyToID("CZY_LuxuryVariation");

            CZY_HeightFogBaseID = Shader.PropertyToID("CZY_HeightFogBase");
            CZY_HeightFogBaseVariationScaleID = Shader.PropertyToID("CZY_HeightFogBaseVariationScale");
            CZY_HeightFogBaseVariationAmountID = Shader.PropertyToID("CZY_HeightFogBaseVariationAmount");
            CZY_HeightFogTransitionID = Shader.PropertyToID("CZY_HeightFogTransition");
            CZY_HeightFogDistanceID = Shader.PropertyToID("CZY_HeightFogDistance");
            CZY_HeightFogColorID = Shader.PropertyToID("CZY_HeightFogColor");
            CZY_HeightFogIntensityID = Shader.PropertyToID("CZY_HeightFogIntensity");

        }
    }
}