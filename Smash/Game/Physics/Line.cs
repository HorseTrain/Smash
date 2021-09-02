using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smash.Game.Physics
{
    public struct LineIntersection
    {
        public bool IsValid;
        public Vector2 Point;
        public Line line0;
        public Line line1;
    }

    //y = mx + b
    public struct InterceptForm
    {
        public float y;
        public float x;
        public float b;

        public InterceptForm(float y, float x, float b)
        {
            this.y = y;
            this.x = x;
            this.b = b;
        }

        public float Evaluate(float m)
        {
            return (m * x) + b;
        }

        public static Vector2 FindIntersection(InterceptForm intercept0, InterceptForm intercept1)
        {
            float x = (-intercept0.b + intercept1.b) / (intercept0.x - intercept1.x);
            float y = intercept0.Evaluate(x);

            return new Vector2(x, y);
        }

        public override string ToString()
        {
            return $"y = {x}x + {b}";
        }
    }

    //ax + by = c
    public struct SlopeForm
    {
        //NOTE: it is assumed that the magnatude of x and y is roughly 1, or else the entire thing breaks.
        public float x;
        public float y;
        public float c;

        public bool IsVertical() => y == 0;

        //Assumes that y != 0
        public InterceptForm GetSlopeInterceptForm()
        {
            Vector3 Out = new Vector3(y, -x, c);

            Out *= (1 / Out.X);

            return new InterceptForm(Out.X, Out.Y, Out.Z);
        }

        public static SlopeForm GetFromLine(Line line)
        {
            SlopeForm Out = new SlopeForm();

            Vector2 Direction = line.Direction;

            Out.y = -Direction.X;
            Out.x = Direction.Y;
            Out.c = (line.Point0.X * Out.x) - (line.Point0.Y * -Out.y);

            return Out;
        }

        //THIS ASSUMES THE MAGNATUDE OF X AND Y IS ROUGHLY 1, AND THE "Other" SLOPE ISN'T VERTICAL
        static Vector2 GetIntersectionFromVerticalLine(SlopeForm vertical, SlopeForm Other)
        {
            //If the assumptions are true, we already know
            //the x coord of the intersection is the vertical
            //line is B. 
            float ox = vertical.c * vertical.x;

            //To calculate Y, ASSUMING the assumptions are true,
            //we must convert the other line into intercept form.
            InterceptForm slope = Other.GetSlopeInterceptForm();

            return new Vector2(ox,(slope.x * ox) + slope.b);
        }

        public static Vector2 CalculateIntersection(SlopeForm slope0, SlopeForm slope1, out bool IsParalell)
        {
            //Check if the lines are paralell. If so, We can exit.
            if (slope0.x == slope1.x && slope0.y == slope1.y)
            {
                IsParalell = true;

                return new Vector2(0,0);
            }

            IsParalell = false;

            //If one line is vertical, we can not use slope intercept
            //form, because we would be deviding by zero :(. But a simple
            //workaround is to check if a line is vertical, and calculate
            //the intersection with some assumptions.
            if (slope0.IsVertical())
            {
                return GetIntersectionFromVerticalLine(slope0, slope1);
            }

            if (slope1.IsVertical())
            {
                return GetIntersectionFromVerticalLine(slope1, slope0);
            }

            //With all the saftey checks out of the way, we can safely
            //calculate the intersection with slope intercept form.
            return InterceptForm.FindIntersection(slope0.GetSlopeInterceptForm(), slope1.GetSlopeInterceptForm());
        }

        public override string ToString()
        {
            return $"{x}x + {y}y = {c}";
        }
    }

    public struct Line
    {
        public Vector2 Point0;
        public Vector2 Point1;

        public Vector2 Direction
        {
            get
            {
                Vector2 dirTemp = Point1 - Point0;

                return dirTemp.Normalized();
            }
        }

        public Vector2 Normal
        {
            get
            {
                Vector2 dirTemp = Direction;

                return new Vector2(dirTemp.Y, dirTemp.X);
            }
        }

        public float Length => (Point1 - Point0).Length;

        public static Line FromTwoPoints(Vector2 Point0, Vector2 Point1)
        {
            return new Line()
            {
                Point0 = Point0,
                Point1 = Point1
            };
        }

        public static Line FromDirection(Vector2 Start, Vector2 Direction)
        {
            return new Line()
            {
                Point0 = Start,
                Point1 = Start + Direction
            };
        }

        public static LineIntersection GetIntersection(Line line0, Line line1)
        {
            LineIntersection Out = new LineIntersection();

            SlopeForm slope0 = SlopeForm.GetFromLine(line0);
            SlopeForm slope1 = SlopeForm.GetFromLine(line1);

            Vector2 point = SlopeForm.CalculateIntersection(slope0, slope1, out bool IsParalell);

            Out.line0 = line0;
            Out.line1 = line1;
            Out.Point = point;
            Out.IsValid = false;

            if (!IsParalell)
            {
                bool PointInLine(Line test, Vector2 point)
                {
                    float FindMin(float test0, float test1)
                    {
                        return test0 < test1 ? test0 : test1;
                    }

                    float FindMax(float test0, float test1)
                    {
                        return test0 > test1 ? test0 : test1;
                    }

                    float xmin = FindMin(test.Point0.X, test.Point1.X);
                    float xmax = FindMax(test.Point0.X, test.Point1.X);

                    float ymin = FindMin(test.Point0.Y, test.Point1.Y);
                    float ymax = FindMax(test.Point0.Y, test.Point1.Y);

                    return (point.X >= xmin && point.X <= xmax) && (point.Y >= ymin && point.Y <= ymax);
                }

                Out.IsValid = PointInLine(line0, point) && PointInLine(line1, point);
            }

            return Out;
        }


    }
}
