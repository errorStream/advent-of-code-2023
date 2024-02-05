using System.Globalization;

namespace Day15
{
    public class Part2 : CommonFunctionality, Framework.ISolution
    {
        public long Run(StreamReader streamReader)
        {
            ArgumentNullException.ThrowIfNull(streamReader);
            var operations = new List<string>();
            while (streamReader.ReadLine() is string line)
            {
                operations.AddRange(line.Split(','));
            }

            var boxes = new List<(string label, int focalLength)>[256];

            for (int i = 0; i < boxes.Length; ++i)
            {
                boxes[i] = new();
            }

            foreach (var item in operations)
            {
                if (item.EndsWith('-'))
                {
                    var label = item[..^1];
                    var boxIndex = HashAlgorithm(label);
                    var box = boxes[boxIndex];
                    var index = box.FindIndex(x => x.label == label);
                    if (index != -1)
                    {
                        box.RemoveAt(index);
                    }
                }
                else
                {
                    var parts = item.Split('=');
                    var label = parts[0];
                    var focalLength = int.Parse(parts[1], CultureInfo.InvariantCulture);
                    var boxIndex = HashAlgorithm(label);
                    var box = boxes[boxIndex];
                    var index = box.FindIndex(x => x.label == label);
                    if (index == -1)
                    {
                        box.Add((label, focalLength));
                    }
                    else
                    {
                        box[index] = (label, focalLength);
                    }
                }
            }

            var total = 0;

            for (int boxNumber = 0; boxNumber < boxes.Length; ++boxNumber)
            {
                var box = boxes[boxNumber];
                for (int slotNumber = 0; slotNumber < box.Count; ++slotNumber)
                {
                    var lense = box[slotNumber];
                    var focusingPower = (boxNumber + 1) * (slotNumber + 1) * lense.focalLength;
                    total += focusingPower;
                }
            }

            return total;
        }
    }
}
