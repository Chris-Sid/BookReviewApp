interface PaginationProps {
  page: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
  disabled?: boolean;
  onPageChange: (page: number) => void;
}

export function Pagination({
  page,
  totalPages,
  hasPrevious,
  hasNext,
  disabled,
  onPageChange,
}: PaginationProps) {
  return (
    <nav className="pagination" aria-label="Book pages">
      <button disabled={!hasPrevious || disabled} onClick={() => onPageChange(page - 1)}>
        Previous
      </button>
      <span aria-live="polite">
        Page {page} of {Math.max(totalPages, 1)}
      </span>
      <button disabled={!hasNext || disabled} onClick={() => onPageChange(page + 1)}>
        Next
      </button>
    </nav>
  );
}