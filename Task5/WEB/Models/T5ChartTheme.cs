using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB.Models
{
    public static class T5ChartTheme
    {
        public const string Vanilla = "<Chart Palette=\"SemiTransparent\" BorderColor=\"#ffffff\" BorderWidth=\"0\" BorderlineDashStyle=\"Solid\">\r\n<ChartAreas>\r\n    <ChartArea _Template_=\"All\" Name=\"Default\">\r\n            <AxisX>\r\n                <MinorGrid Enabled=\"False\" />\r\n                <MajorGrid Enabled=\"False\" />\r\n            </AxisX>\r\n            <AxisY>\r\n                <MajorGrid Enabled=\"False\" />\r\n                <MinorGrid Enabled=\"False\" />\r\n            </AxisY>\r\n    </ChartArea>\r\n</ChartAreas>\r\n</Chart>";
        public const string Vanilla3D = "<Chart BackColor=\"#555\" BackGradientStyle=\"TopBottom\" BorderColor=\"181, 64, 1\" BorderWidth=\"2\" BorderlineDashStyle=\"Solid\" Palette=\"SemiTransparent\" AntiAliasing=\"All\">\r\n    <ChartAreas>\r\n        <ChartArea Name=\"Default\" _Template_=\"All\" BackColor=\"Transparent\" BackSecondaryColor=\"White\" BorderColor=\"64, 64, 64, 64\" BorderDashStyle=\"Solid\" ShadowColor=\"Transparent\">\r\n            <Area3DStyle LightStyle=\"Simplistic\" Enable3D=\"True\" Inclination=\"0\" IsClustered=\"False\" IsRightAngleAxes=\"False\" Perspective=\"10\" Rotation=\"0\" WallWidth=\"0\" />\r\n        </ChartArea>\r\n    </ChartAreas>\r\n</Chart>";

    }
}