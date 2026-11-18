import { useState } from 'react';
import { useBooks } from '../hooks/useBooks';
import { useBookReviews } from '../hooks/useBookReviews';
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
        placeholder="Filter by genre"
        aria-label="Filter by genre"
      />

      {isPending && <p role="status">Loading books</p>}

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

export function ReviewList({ bookId }: { bookId: string }) {
    const { data, isPending, isError, error, refetch } = useBookReviews(bookId, true);

    if (isPending) return <p role="status">Loading reviews...</p>;

    if (isError) {
        return (
            <div role="alert">
                <p>{error.message}</p>
                <button type="button" onClick={() => refetch()}>Retry</button>
            </div>
        );
    }

    if (data.length === 0) return <p>No reviews yet.</p>;

    return (
        <ul className="review-list">
            {data.map((r) => (
                <li key={r.id}>
                    <strong>{'*'.repeat(r.rating)}</strong> ({r.rating}/5)
                    <p>{r.content}</p>
                    <small>
                        {r.userName ?? 'Anonymous'} - {new Date(r.dateCreated).toLocaleDateString()}
                    </small>
                </li>
            ))}
        </ul>
    );
}