export interface Book {
  id: string;
  title: string;
  author: string;
  publishedYear: number;
  genre: string;
}

export interface PagedResult<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

export interface BookQuery {
  genre?: string;
  year?: number;
  page: number;
  pageSize: number;
}