using Flunklug.Behaviours;
using GorillaExtensions;
using GorillaInfoWatch.Models;
using GorillaInfoWatch.Models.Attributes;
using GorillaInfoWatch.Models.Widgets;
using System.Linq;
using UnityEngine;
using Player = GorillaLocomotion.GTPlayer;

[assembly: InfoWatchCompatible]

namespace Flunklug.Models
{
    [ShowOnHomeScreen(DisplayTitle = "Flunklug")]
    internal class MainFlunklugScreen : InfoScreen
    {
        public override string Title => "Flunklug (pl2w)";

        public override InfoContent GetContent()
        {
            PageBuilder pages = new();

            LineBuilder lines = new();
            lines.Add("Generate a \"Flunklug\"", new Widget_PushButton(SelectOption, 0));
            lines.Add("Masskill Flunklugs", new Widget_PushButton(SelectOption, 1));
            lines.Add("Teleport All", new Widget_PushButton(SelectOption, 2));
            pages.AddPage("Options", lines);

            lines = new();
            if (FlunkController.lugs.Count != 0)
            {
                foreach (var flunklugBall in FlunkController.lugs)
                {
                    lines.Append(flunklugBall.flunkName).Append(string.Format(
                        " [{0}, {1}, {2}]",
                        Mathf.RoundToInt(flunklugBall.color.r * 10f),
                        Mathf.RoundToInt(flunklugBall.color.g * 10f),
                        Mathf.RoundToInt(flunklugBall.color.b * 10f)))
                    .Add(new Widget_PushButton(SelectFlunklug, flunklugBall));
                }
            }
            else
            {
                lines.BeginCentre().Append("<color=red>No flunklugs have been spawned</color>").EndAlign().AppendLine();
            }

            pages.AddPage("Collection", lines);

            return pages;
        }

        public void SelectOption(object[] args)
        {
            if (args.ElementAtOrDefault(0) is int option)
            {
                switch (option)
                {
                    case 0:
                        FlunklugBall flunklugBall = FlunkController.Instance.GenerateFlunklug(0);
                        flunklugBall.Teleport(Player.Instance.HeadCenterPosition + Random.insideUnitSphere.WithY(0), false);
                        flunklugBall.PlayBirthSound();
                        SetContent();
                        break;
                    case 1:
                        FlunkController.lugs.ForEach(lug => FlunkController.Instance.DestroyBall(lug, false));
                        FlunkController.lugs.Clear();
                        SetContent();
                        break;
                    case 2:
                        FlunkController.lugs.ForEach(lug => lug.Teleport(Player.Instance.HeadCenterPosition + Random.insideUnitSphere.WithY(0), true));
                        break;
                }
            }
        }

        public void SelectFlunklug(object[] args)
        {
            if (args.ElementAtOrDefault(0) is FlunklugBall flunklug && flunklug)
            {
                SelectedFlunklugScreen.ball = flunklug;
                LoadScreen<SelectedFlunklugScreen>();
            }
        }
    }
}
