import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { QueryClient, QueryClientProvider, QueryCache } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { BookList } from './components/BookList';
import { fetchBookReviews } from './api/booksApi';
import './index.css';

const queryClient = new QueryClient({
  queryCache: new QueryCache({
    onSuccess: (_data, query) => console.log('[query ok]', JSON.stringify(query.queryKey)),
  }),
  defaultOptions: { queries: { retry: 1, refetchOnWindowFocus: false } },
});

if (import.meta.env.DEV) {
  (window as unknown as { queryClient: QueryClient }).queryClient = queryClient;
}

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <main>
        <h1>Book Reviews</h1>
        <BookList />
      </main>
       <ReactQueryDevtools initialIsOpen={false} />
    </QueryClientProvider>
  </StrictMode>,
);
