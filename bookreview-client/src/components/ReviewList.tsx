import { useBookReviews } from '../hooks/useBookReviews';

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