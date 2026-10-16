import { memo } from 'react';
import type { Book } from '../types/book';

interface BookCardProps {
  book: Book;
}

function BookCardComponent({ book }: BookCardProps) {
  return (
    <article className="book-card">
      <h3>{book.title}</h3>
      <p className="author">by {book.author}</p>
      <p className="meta">
        <span className="genre">{book.genre}</span>
        <span>{book.publishedYear}</span>
      </p>
    </article>
  );
}

export const BookCard = memo(BookCardComponent);