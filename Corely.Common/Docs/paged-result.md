# Paged Result

`PagedResult<T>` is a class that helps in managing paginated data. It provides properties and methods to handle pagination logic, such as skipping items, taking a specific number of items, and determining if there are more items to fetch.

## Features

- Manage pagination state
- Delegate handling for fetching next page of data
- Track whether more data is available

## Usage

Create a new paged result with a page size of 10 items, starting at page 0, and get the first page.

```csharp
var pagedResult = new PagedResult<MyItem>(0, 10); 
pagedResult.OnGetNextPage += (pagedResponse) =>
{
    pagedResponse.SetItems(
        _dataSource
            .Skip(pagedResponse.Skip)
            .Take(pagedResponse.Take));
    return pagedResponse;
}; 
var firstPage = pagedResult.GetNextPage();
```


### Properties
- `Items`: The list of items in the current page.
- `Skip`: The number of items to skip.
- `Take`: The number of items to take.
- `PageNum`: The current page number.
- `HasMore`: Indicates if there are more items to fetch.

### Methods
- `GetNextPage()`: Fetches the next page of items.
- `SetItems(IEnumerable<T> items)`: Sets the items for the current page.
- `AddItems(IEnumerable<T> items)`: Adds items for the current page.

### Events
- `OnGetNextPage`: An event that is triggered to fetch the next page of items when `GetNextPage` is called.
  - Can only be set once to avoid multiple event handlers being added.




