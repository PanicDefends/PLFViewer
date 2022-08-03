using PFLViewer.Model.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFLViewer.Model.Model
{
    public class IPointModelComparer : IComparer<IPointModel>
    {
        private enum EditInfoStatusPair
        {
            NoEdits,
            BothAdded,
            BothEdited,
            OneAdded,
            OneEdited,
            OneAddedOneEdited
        }

        protected IList<IPointModel> _points;
        protected IList<PointEditInfo> _editedPoints;

        public IPointModelComparer(IList<IPointModel> points, IList<PointEditInfo> editedPoints)
        {
            _points = points;
            _editedPoints = editedPoints;
        }

        public int Compare(IPointModel p1, IPointModel p2)
        {
            if (p1 == null && p2 == null)
                return 0;
            if (p1 == null || p2 == null)
                return p1 != null ? -1 : 1;

            if (p1.X == p2.X && p1.Y == p2.Y)
                return 0;

            if (p1.X != p2.X)
                return p1.X.CompareTo(p2.X);


            PointEditInfo info1 = GetEditInfo(p1);
            PointEditInfo info2 = GetEditInfo(p2);

            var editStatusPair = GetEditStatusPair(info1, info2);

            switch (editStatusPair)
            {
                case EditInfoStatusPair.NoEdits:
                case EditInfoStatusPair.BothEdited:
                    return KeepOrder(p1, p2);

                case EditInfoStatusPair.BothAdded:
                    return CompareYValues(p1, p2);

                case EditInfoStatusPair.OneAddedOneEdited:
                case EditInfoStatusPair.OneAdded:
                    return SetAddedRighter(info1, info2);

                case EditInfoStatusPair.OneEdited:
                    return CompareOldValues(p1, info1, p2, info2);
            }

            return 0;
        }

        protected PointEditInfo GetEditInfo(IPointModel point)
        {
            return _editedPoints.FirstOrDefault(e => e.Point == point);
        }

        private EditInfoStatusPair GetEditStatusPair(PointEditInfo info1, PointEditInfo info2)
        {
            if (info1 == null && info2 == null)
                return EditInfoStatusPair.NoEdits;

            if (info1?.IsAdded == true && info2?.IsAdded == true)
                return EditInfoStatusPair.BothAdded;

            if (info1?.IsAdded == false && info2?.IsAdded == false)
                return EditInfoStatusPair.BothEdited;

            if (info1 != null && info2 != null)
                return EditInfoStatusPair.OneAddedOneEdited;

            if (info1?.IsAdded == true ^ info2?.IsAdded == true)
                return EditInfoStatusPair.OneAdded;

            return EditInfoStatusPair.OneEdited;
        }
        private int KeepOrder(IPointModel p1, IPointModel p2)
        {
            return _points.IndexOf(p1).CompareTo(_points.IndexOf(p2));
        }

        private int CompareYValues(IPointModel p1, IPointModel p2)
        {
            return p1.Y.CompareTo(p2.Y);
        }
        private int SetAddedRighter(PointEditInfo info1, PointEditInfo info2)
        {
            return info1?.IsAdded == true ? 1 : -1;
        }

        private int CompareOldValues(IPointModel p1, PointEditInfo info1, IPointModel p2, PointEditInfo info2)
        {
            var value1 = info1?.OldXValue.HasValue == true ? info1.OldXValue.Value : p1.X;
            var value2 = info2?.OldXValue.HasValue == true ? info2.OldXValue.Value : p2.X;

            var compareResult = value1.CompareTo(value2);

            return compareResult != 0 ? compareResult : KeepOrder(p1, p2);
        }
    }


    public class PointEditInfo
    {
        protected IPointModel _point;

        public PointEditInfo(IPointModel point, double? oldXValue = null)
        {
            _point = point;
            OldXValue = oldXValue;
        }

        public IPointModel Point => _point;
        public double? OldXValue { get; }
        public bool IsAdded => !OldXValue.HasValue;
    }
}
