using System;
using System.Collections.Generic;
using System.Linq;
using Funcan.Domain.Models;
using PointSet = Funcan.Domain.Models.PointSet;

namespace Funcan.Domain.Plotters;

public class FunctionPlotter : IPlotter
{
    private DiscontinuitiesPlotter DiscontinuitiesPlotter { get; }

    public FunctionPlotter(DiscontinuitiesPlotter discontinuitiesPlotter) =>
        DiscontinuitiesPlotter = discontinuitiesPlotter;

    public PlotterInfo PlotterInfo => new("function", DrawType.Line);

    public IEnumerable<PointSet> GetPointSets(Func<double, double> function, FunctionRange functionRange)
    {
        var discontinuities = DiscontinuitiesPlotter
            .GetPointSets(function, functionRange).First().Points.ToHashSet();

        var points = new PointSet();

        for (var x = functionRange.From; x <= functionRange.To; x += Settings.Step)
        {
            var y = function(x);
            var point = new Point(x, y);
            if (discontinuities.Contains(point))
            {
                yield return points;
                points = new PointSet();
            }
            else
            {
                points.Add(new Point(x, y));
            }

            if (points.Points.Any()) yield return points;
        }
    }

    // public PointSet GetPointSets(Func<double, double> function, FunctionRange functionRange)
    // {
    //     var points = new PointSet(new Style(new Color("Blue"), Style.DrawType.Line));
    //     for (var x = functionRange.From; x <= functionRange.To; x += 0.2d)
    //     {
    //         var y = function(x);
    //         points.Add(new Point(x, y));
    //     }
    //     return points;
    // }
}