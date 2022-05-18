using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace ShapesBalanceXamFormsApp
{
    public partial class MainPage : ContentPage
    {



        public class Wallet
        {
            public Wallet(double value, Brush color)
            {
                CryptoValue = value;
                Stroke = color;
            }

            public double CryptoValue { get; private set; }
            public Brush Stroke { get; private set; }
        }

        public IEnumerable<Wallet> GetAmount()
        {

            yield return new Wallet(1000, Brush.Green);
            yield return new Wallet(50, Brush.Black);
            yield return new Wallet(600, Brush.Blue);
            yield return new Wallet(50, Brush.Gray);
            yield return new Wallet(250, Brush.Brown);
            yield return new Wallet(10, Brush.Silver);

        }

        public MainPage()
        {
            InitializeComponent();
                    
            
            IEnumerable<Wallet> amounts = GetAmount();
            double amount = 0;

            foreach (Wallet bal in amounts)
            {
                amount += bal.CryptoValue;
            }

            Grid grid = new Grid
            {
                RowDefinitions = { new RowDefinition() },
                ColumnDefinitions = { new ColumnDefinition() },

            };

            Label currency = new Label
            {
                Text = "€",
                FontAttributes = FontAttributes.Bold,
                FontSize = 16,
                HorizontalOptions = LayoutOptions.Center,
                TranslationY = 160,
                TranslationX = -30
            };

            grid.Children.Add(currency);

            Label balance = new Label
            {
                Text = amount.ToString(),
                FontSize = 25,
                HorizontalOptions = LayoutOptions.Center,
                TranslationY = 160
            };

            grid.Children.Add(balance);

            Label total_tag = new Label
            {
                Text = "Amount Balance",  FontAttributes = FontAttributes.Bold,
                FontSize = 15,
                HorizontalOptions = LayoutOptions.Center,
                TranslationY = 200
            };

            grid.Children.Add(total_tag);
            makePies(grid, amounts);
            Content = grid;



        }


        public void makePies(Grid grid, IEnumerable<Wallet> amounts)
        {

            int i; 
            int n = amounts.Count();
            double change, hold;
            double arcAngle = 0;
            double total = 0;
            double normalize = 1;
                  

            
            foreach (Wallet bal in amounts)
            {
                
                total += bal.CryptoValue;
            }

            for (i = 0; i < n; i++)
            {
                normalize = 1;
                hold = amounts.ElementAt(i).CryptoValue;
                change = ( hold/ total) * 360 ;

                
                Path path = new Path { Stroke = amounts.ElementAt(i).Stroke };
                PathGeometry geometry = new PathGeometry();
                PathFigureCollection pathFigures = new PathFigureCollection();
                PathFigure pathFigure = new PathFigure();
                PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();
                ArcSegment arcSegment = new ArcSegment();

                if(change <= 10 )
                {
                    normalize = 0.1;
                    arcSegment = renderArc(path, pathFigure, arcSegment, arcAngle + normalize , change - normalize * 2 );

                }
                else
                {
                    normalize = 10;
                    arcSegment = renderArc(path, pathFigure, arcSegment, arcAngle + normalize, change - normalize * 2);

                }

                path.Data = geometry;
                geometry.Figures = pathFigures;
                pathFigures.Add(pathFigure);
                pathFigure.Segments.Add(arcSegment);

                grid.Children.Add(path);

                //if (change >= 10)
                    arcAngle = arcAngle + change;

            }

        }

 

        private ArcSegment renderArc(Path pathRoot, PathFigure pathFigure, ArcSegment ARC, double startAngle, double endAngle)
        {
            double Radius = 150;
            double angle = 0;
            bool largeArc = false;

            if (endAngle > 180)
                largeArc = true;    

            pathRoot.StrokeLineCap = PenLineCap.Round;
            pathRoot.StrokeThickness = 12;
            ARC.SweepDirection = SweepDirection.Clockwise;
            ARC.Size = new Size(Radius, Radius);
            ARC.RotationAngle = angle;
            ARC.IsLargeArc = largeArc;
            //////////////////////////
            pathFigure.StartPoint = ComputeCartesianCoordinate(startAngle, Radius);
            ARC.Point = ComputeCartesianCoordinate(startAngle + endAngle, Radius);

            return ARC;

        }

        private Point ComputeCartesianCoordinate(double angle, double radius)
        {
            //center (50,50)
            double cX = 50;
            double cY = 50;
            // convert to radians
            double angleRad = (Math.PI / 180.0) * (angle - 90);

            double x = radius * Math.Cos(angleRad);
            double y = radius * Math.Sin(angleRad);

            x += radius + cX;
            y += radius + cY;

            return new Point(x, y);

        }


    }
}

