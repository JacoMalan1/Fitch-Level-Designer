using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitch_Level_Designer
{
    public enum TweenType
    {
        Instant,
        Linear,
        QuadraticInOut,
        CubicInOut,
        QuarticOut
    }

    class View
    {
        private Vector2 position;
        public float zoom;
        public double rotation;

        private Vector2 positionGoto, positionFrom;
        private TweenType tweenType;
        private float tweenSteps, currentStep;

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }
        public Vector2 PositionGoto
        {
            get
            {
                return positionGoto;
            }
        }

        /// <param name="pos">Needs to be relative to the screen center!</param>
        /// <returns>The coorisponding position in the world</returns>
        public Vector2 ToWorld(Vector2 pos)
        {
            pos /= zoom;
            Vector2 dX = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
            Vector2 dY = new Vector2(
                (float)Math.Cos(rotation + Math.PI / 2.0),
                (float)Math.Sin(rotation + Math.PI / 2.0));
            return (this.position + (dX * pos.X + dY * pos.Y));
        }

        public View(Vector2 startPosition, float startZoom = 1f, double rotation = 0.0)
        {
            this.position = startPosition;
            this.positionGoto = startPosition;
            this.positionFrom = startPosition;
            this.zoom = startZoom;
            this.rotation = rotation;
        }

        public void Update()
        {
            if (currentStep < tweenSteps)
            {
                currentStep++;

                switch (tweenType)
                {
                    case TweenType.Instant:
                        this.position = positionGoto;
                        break;
                    case TweenType.Linear:
                        this.position = positionFrom + (positionGoto - positionFrom) *
                             GetLinear((float)this.currentStep / this.tweenSteps);
                        break;
                    case TweenType.CubicInOut:
                        this.position = positionFrom + (positionGoto - positionFrom) *
                            GetCubicInOut((float)this.currentStep / this.tweenSteps);
                        break;
                    case TweenType.QuadraticInOut:
                        this.position = positionFrom + (positionGoto - positionFrom) *
                            GetQuadraticInOut((float)this.currentStep / this.tweenSteps);
                        break;
                    case TweenType.QuarticOut:
                        this.position = positionFrom + (positionGoto - positionFrom) *
                            GetQuarticOut((float)this.currentStep / this.tweenSteps);
                        break;
                    default:
                        //We won't do anything
                        break;
                }

            }
            else
            {
                this.position = positionGoto;
            }
        }

        public void SetPosition(Vector2 newPosition)
        {
            this.positionFrom = this.position;
            this.positionGoto = newPosition;
            this.position = newPosition;
            this.tweenType = TweenType.Instant;
            this.currentStep = 0;
            this.tweenSteps = 0;
        }
        public void SetPosition(Vector2 newPosition, TweenType tweenType, int numSteps)
        {
            if (newPosition != this.position)
            {
                this.positionFrom = this.position;
                this.positionGoto = newPosition;
                this.tweenType = tweenType;
                this.currentStep = 0;
                this.tweenSteps = numSteps;
            }
        }

        private float GetLinear(float t)
        {
            return t;
        }
        private float GetQuadraticInOut(float t)
        {
            return (t * t) / ((2 * t * t) - (2 * t) + 1);
        }
        private float GetQuarticOut(float t)
        {
            return -((t - 1) * (t - 1) * (t - 1) * (t - 1)) + 1;
        }
        private float GetCubicInOut(float t)
        {
            return (t * t * t) / ((3 * t * t) - (3 * t) + 1);
        }

        public void ApplyTransforms()
        {
            Matrix4 transform = Matrix4.Identity;

            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(-position.X, -position.Y, 0));
            transform = Matrix4.Mult(transform, Matrix4.CreateRotationZ(-(float)rotation));
            transform = Matrix4.Mult(transform, Matrix4.CreateScale(zoom, zoom, 1.0f));

            GL.MultMatrix(ref transform);
        }
    }
}
