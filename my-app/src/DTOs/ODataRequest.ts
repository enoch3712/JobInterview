export default interface ODataRequest<T> {
    $id: string;
    $values: T[];
}