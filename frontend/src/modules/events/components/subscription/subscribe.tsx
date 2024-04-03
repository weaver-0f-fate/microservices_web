import React, { useRef } from 'react';
import { Button } from '@mui/material';
import SubscribeDialog from './subscribeDialog';
import { useSelector } from "react-redux";
import { SelectedEventState } from '../../store/selectedEventSlice';

const Recurrency = () => {
    const subscribeDialogRef = useRef<any>({});
    const event = useSelector((state: any) => state.selectedEvent as SelectedEventState);
    const disabled = event.event.uuid ? false : true;

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