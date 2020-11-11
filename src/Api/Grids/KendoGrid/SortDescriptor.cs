namespace SharedKernel.Api.Grids.KendoGrid
{
    /// <summary>
    /// 
    /// </summary>
    public class SortDescriptor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="dir"></param>
        public SortDescriptor(string field, string dir)
        {
            Field = field;
            Dir = dir;
        }

        /// <summary>
        /// The field that is sorted.
        /// </summary>
        public string Field { get; }

        /// <summary>
        /// The sort direction. If no direction is set, the descriptor will be skipped during processing.
        ///
        /// The available values are:
        /// - `asc`
        /// - `desc`
        /// </summary>
        public string Dir { get; }
    }
}