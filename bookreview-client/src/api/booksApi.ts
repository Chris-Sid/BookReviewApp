import type { Book, BookQuery, PagedResult } from '../types/book';

export class ApiError extends Error {
  constructor(public status: number, message: string) {
    super(message);
  }
}

export async function fetchBooks(
  query: BookQuery,
  signal?: AbortSignal
): Promise<PagedResult<Book>> {
  const params = new URLSearchParams({
    page: String(query.page),
    pageSize: String(query.pageSize),
  });
  if (query.genre) params.set('genre', query.genre);
  if (query.year) params.set('year', String(query.year));

  const response = await fetch(`/api/books/paged?${params}`, { signal });
  if (!response.ok) {
    throw new ApiError(response.status, `Failed to load books (${response.status})`);
  }
  return response.json() as Promise<PagedResult<Book>>;
}