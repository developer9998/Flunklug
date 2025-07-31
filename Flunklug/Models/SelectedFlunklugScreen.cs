using Flunklug.Behaviours;
using GorillaInfoWatch.Models;
using GorillaInfoWatch.Models.Widgets;
using System;
using System.Linq;
using UnityEngine;
using Player = GorillaLocomotion.GTPlayer;

namespace Flunklug.Models
{
    public class SelectedFlunklugScreen : InfoWatchScreen
    {
        public static FlunklugBall ball;

        public override string Title => $"<color=#{ColorUtility.ToHtmlStringRGB(ball.color)}>{ball.flunkName}</color>";

        public override ScreenContent GetContent()
        {
            LineBuilder lines = new();

            lines.Add("Teleport", new Widget_PushButton(SelectPushButton, 0));
            lines.Add($"Kill {ball.flunkName}", new Widget_PushButton(SelectPushButton, 1));
            lines.Skip();
            lines.Add($"Throw Strength: {Math.Round(ball.throwStrength, 1)}", new Widget_PushButton(SelectPushButton, 2, -1f)
            {
                Colour = ColourPalette.Red,
                Symbol = InfoWatchSymbol.Stop
            }, new Widget_PushButton(SelectPushButton, 2, 1f)
            {
                Colour = ColourPalette.Green,
                Symbol = InfoWatchSymbol.Play
            });
            lines.Add($"Static Friction: {Math.Round(ball.material.staticFriction, 1)}", new Widget_PushButton(SelectPushButton, 3, -1f)
            {
                Colour = ColourPalette.Red,
                Symbol = InfoWatchSymbol.Stop
            }, new Widget_PushButton(SelectPushButton, 3, 1f)
            {
                Colour = ColourPalette.Green,
                Symbol = InfoWatchSymbol.Play
            });
            lines.Add($"Style: {(FlunkController.stylesAndNames.TryGetValue(ball.style, out string value) ? value : "N/A")}", new Widget_SnapSlider(ball.style, 0, FlunkController.stylesAndNames.Count - 1, AdjustSlider)
            {
                Colour = ColourPalette.CreatePalette(ColourPalette.Button.Evaluate(0))
            });

            return lines;
        }

        public void SelectPushButton(object[] args)
        {
            if (args.ElementAtOrDefault(0) is int option)
            {
                switch (option)
                {
                    case 0:
                        ball.Teleport(Player.Instance.HeadCenterPosition, true);
                        break;
                    case 1:
                        FlunkController.Instance.DestroyBall(ball, true);
                        SetScreen<MainFlunklugScreen>();
                        break;
                    case 2:
                        ball.throwStrength = Mathf.Max(ball.throwStrength + (0.1f * (float)args[1]), 0f);
                        SetText();
                        break;
                    case 3:
                        ball.ChangePhysics(ball.material.bounciness, Mathf.Max(ball.material.staticFriction + (0.5f * (float)args[1]), 0f));
                        SetText();
                        break;
                }
            }
        }

        public void AdjustSlider(int value)
        {
            if (FlunkController.stylesAndNames.ContainsKey(value))
            {
                ball.SetStyle(value);
                SetText();
            }
        }
    }
}
