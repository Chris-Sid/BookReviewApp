import { useState } from 'react';
import { useBooks } from '../hooks/useBooks';
import { useDebouncedValue } from '../hooks/useDebouncedValue';
import { BookCard } from './BookCard';
import { Pagination } from './Pagination';

const PAGE_SIZE = 5;

export function BookList() {
  const [genreInput, setGenreInput] = useState('');
  const [page, setPage] = useState(1);
  const genre = useDebouncedValue(genreInput.trim());

  const { data, isPending, isError, error, isFetching, refetch } = useBooks({
    genre: genre || undefined,
    page,
    pageSize: PAGE_SIZE,
  });

  const handleGenreChange = (value: string) => {
    setGenreInput(value);
    setPage(1); // reset paging whenever the filter changes
  };

  return (
    <section>
      <input
        type="search"
        value={genreInput}
        onChange={(e) => handleGenreChange(e.target.value)}
        placeholder="Filter by genre…"
        aria-label="Filter by genre"
      />

      {isPending && <p role="status">Loading books…</p>}

      {isError && (
        <div role="alert">
          <p>{error.message}</p>
          <button onClick={() => refetch()}>Retry</button>
        </div>
      )}

      {data && (
        <>
          {data.items.length === 0 ? (
            <p>No books found.</p>
          ) : (
            <div className="book-grid" aria-busy={isFetching}>
              {data.items.map((book) => (
                <BookCard key={book.id} book={book} />
              ))}
            </div>
          )}

          <Pagination
            page={data.page}
            totalPages={data.totalPages}
            hasPrevious={data.hasPrevious}
            hasNext={data.hasNext}
            disabled={isFetching}
            onPageChange={setPage}
          />
        </>
      )}
    </section>
  );
}