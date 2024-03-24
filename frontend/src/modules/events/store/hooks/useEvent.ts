import { useTrackedState } from '../store';

const useEvent = () => {
    const state = useTrackedState();
    return state;
}

export default useEvent;