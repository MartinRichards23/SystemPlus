namespace SystemPlus
{
    public static class Parser
    {
        public static int? Int(string s)
        {
            if (int.TryParse(s, out int val))
                return val;

            return null;
        }

        public static double? Double(string s)
        {
            if (double.TryParse(s, out double val))
                return val;

            return null;
        }

        public static float? Float(string s)
        {
            if (float.TryParse(s, out float val))
                return val;

            return null;
        }
    }
}
