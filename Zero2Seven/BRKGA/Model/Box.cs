namespace BRKGA.Model
{
    public class Box
    {
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Value { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public EmptySpace GetSpace()
        {
            return new EmptySpace(X, Y, Z, X + Length, Y + Width, Z + Height);
        }
    }
}
