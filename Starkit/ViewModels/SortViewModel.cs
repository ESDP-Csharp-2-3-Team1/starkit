namespace Starkit.ViewModels
{
    public enum SortState
    {
        NameAsc,
        NameDesc,
        CostAsc,
        CostDesc,
        AddTimeAsc,
        AddTimeDesc,
        CategoryAsc,
        CategoryDesc,
        CalorieAsc,
        CalorieDesc,
        DateAsc,
        DateDesc,
        TimeAsc,
        TimeDesc,
        PaxAsc,
        PaxDesc,
        IdAsc,
        IdDesc
    }
    public class SortViewModel
    {
        public SortState NameSort { get; set; }
        public SortState CostSort { get; set; }
        public SortState AddTimeSort { get; set; }
        public SortState CategorySort { get; set; }
        public SortState CalorieSort { get; set; }
        public SortState Current { get; set; }
        public SortState DateSort { get; set; }
        public SortState TimeSort { get; set; }
        public SortState PaxSort { get; set; }
        public SortState IdSort { get; set; }

        public SortViewModel(SortState sortOrder)
        {
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            CostSort = sortOrder == SortState.CostAsc ? SortState.CostDesc : SortState.CostAsc;
            AddTimeSort = sortOrder == SortState.AddTimeAsc ? SortState.AddTimeDesc : SortState.AddTimeAsc;
            CategorySort = sortOrder == SortState.CategoryAsc ? SortState.CategoryDesc : SortState.CategoryAsc;
            CalorieSort = sortOrder == SortState.CalorieAsc ? SortState.CalorieDesc : SortState.CalorieAsc;
            DateSort = sortOrder == SortState.DateAsc ? SortState.DateDesc : SortState.DateAsc;
            TimeSort = sortOrder == SortState.TimeAsc ? SortState.TimeDesc : SortState.TimeAsc;
            PaxSort = sortOrder == SortState.PaxAsc ? SortState.PaxDesc : SortState.PaxAsc;
            IdSort = sortOrder == SortState.IdAsc ? SortState.IdDesc : SortState.IdAsc;
            Current = sortOrder;
        }
    }
}