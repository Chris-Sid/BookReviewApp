import { memo, useState } from 'react';
import type { Book } from './types/book';
import { ReviewList } from './ReviewList';

interface BookCardProps {
  book: Book;
}

function BookCardComponent({ book }: BookCardProps) {
  const [open, setOpen] = useState(false);
  const panelId = `reviews-${book.id}`;

  return (
    <article className="book-card">
      <button
        type="button"
        className="book-card-toggle"
        aria-expanded={open}
        aria-controls={panelId}
        onClick={() => setOpen((o) => !o)}
      >
        <h3>{book.title}</h3>
        <p className="author">by {book.author}</p>
        <p className="meta">
          <span className="genre">{book.genre}</span>
          <span>{book.publishedYear}</span>
        </p>
      </button>

      {open && (
        <div id={panelId} className="reviews">
          <ReviewList bookId={book.id} />
        </div>
      )}
    </article>
  );
}

export const BookCard = memo(BookCardComponent);