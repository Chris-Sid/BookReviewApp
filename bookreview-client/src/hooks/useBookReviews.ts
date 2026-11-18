import { useQuery } from '@tanstack/react-query';
import { fetchBookReviews } from '../api/booksApi';

export function useBookReviews(bookId: string, enabled: boolean) {
    return useQuery({
        queryKey: ['books', 'reviews', bookId],
        queryFn: ({ signal }) => fetchBookReviews(bookId, signal),
        enabled,
        staleTime: 30_000,
    });
}