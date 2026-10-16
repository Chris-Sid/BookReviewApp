import { keepPreviousData, useQuery } from '@tanstack/react-query';
import { fetchBooks } from '../api/booksApi';
import type { BookQuery } from '../types/book';

export function useBooks(query: BookQuery) {
  return useQuery({
    queryKey: ['books', query],
    queryFn: ({ signal }) => fetchBooks(query, signal),
    placeholderData: keepPreviousData,
    staleTime: 30_000,
  });
}