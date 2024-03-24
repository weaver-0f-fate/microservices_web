import React, { useRef } from 'react';
import { Button } from '@mui/material';
import useEvent from '../../store/hooks/useEvent';
import SubscribeDialog from './subscribeDialog';

const Recurrency = () => {
    const subscribeDialogRef = useRef<any>({});
    const eventStore = useEvent();
    const disabled = eventStore.selectedEvent.uuid ? false : true;

    const handleSubscribe = () => {
        subscribeDialogRef.current.openDialog();
    }

    return (
        <>
            <Button onClick={handleSubscribe} disabled={disabled}>
                Subscribe
            </Button>
            <SubscribeDialog ref={subscribeDialogRef} />
        </>
    )
}

export default Recurrency;