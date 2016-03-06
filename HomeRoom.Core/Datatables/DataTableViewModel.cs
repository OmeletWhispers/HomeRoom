using HomeRoom.Enumerations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeRoom.Datatables
{
    public class ColumnViewModel
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public string Data { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the searchable.
        /// </summary>
        /// <value>
        /// The searchable.
        /// </value>
        public bool Searchable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ColumnViewModel"/> is orderable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if orderable; otherwise, <c>false</c>.
        /// </value>
        public bool Orderable { get; set; }

        /// <summary>
        /// Gets or sets the search.
        /// </summary>
        /// <value>
        /// The search.
        /// </value>
        public SearchViewModel Search { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is ordered.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is ordered; otherwise, <c>false</c>.
        /// </value>
        public bool IsOrdered
        {
            get { return OrderNumber != -1; }
        }

        /// <summary>
        /// Gets or sets the order number.
        /// </summary>
        /// <value>
        /// The order number.
        /// </value>
        public int OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the order direction.
        /// </summary>
        /// <value>
        /// The order direction.
        /// </value>
        public OrderDirection OrderDirection { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnViewModel"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="name">The name.</param>
        /// <param name="searchable">if set to <c>true</c> [searchable].</param>
        /// <param name="orderable">if set to <c>true</c> [orderable].</param>
        /// <param name="searchValue">The search value.</param>
        /// <param name="isRegexValue">if set to <c>true</c> [is regex value].</param>
        public ColumnViewModel(string data, string name, bool searchable, bool orderable, string searchValue, bool isRegexValue)
        {
            Data = data;
            Name = name;
            Searchable = searchable;
            Orderable = orderable;
            Search = new SearchViewModel(searchValue, isRegexValue);
        }

        /// <summary>
        /// Sets the order direction.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <param name="orderDirection">The order direction.</param>
        public void SetOrderDirection(int orderNumber, string orderDirection)
        {
            OrderNumber = orderNumber;

            if (orderDirection.ToLower().Equals("asc"))
            {
                OrderDirection = OrderDirection.Ascendant;
            }
            else
            {
                OrderDirection = OrderDirection.Descendant;
            }
        }

    }
    public class SearchViewModel
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is regex.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is regex; otherwise, <c>false</c>.
        /// </value>
        public bool IsRegex { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchViewModel"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="isRegex">if set to <c>true</c> [is regex].</param>
        public SearchViewModel(string value, bool isRegex)
        {
            Value = value;
            IsRegex = isRegex;
        }
    }

    public class ColumnData : IEnumerable<ColumnViewModel>
    {
        /// <summary>
        /// The data
        /// </summary>
        private readonly IReadOnlyCollection<ColumnViewModel> Data;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnData"/> class.
        /// </summary>
        /// <param name="columnViewModels">The column view models.</param>
        public ColumnData(IEnumerable<ColumnViewModel> columnViewModels)
        {
            Data = columnViewModels.ToList().AsReadOnly();
        }

        /// <summary>
        /// Gets the columns sorted.
        /// </summary>
        /// <returns></returns>
        public IOrderedEnumerable<ColumnViewModel> GetColumnsSorted()
        {
            return Data.Where(x => !string.IsNullOrWhiteSpace(x.Data) && x.IsOrdered).OrderBy(x => x.OrderNumber);
        }

        /// <summary>
        /// Gets the columns filtered.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ColumnViewModel> GetColumnsFiltered()
        {
            return Data
                .Where(x => !string.IsNullOrWhiteSpace(x.Data) && x.Searchable && !string.IsNullOrWhiteSpace(x.Search.Value));
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<ColumnViewModel> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Data).GetEnumerator();
        }
    }

}
