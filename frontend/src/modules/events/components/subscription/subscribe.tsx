import React, { useRef } from 'react';
import { Button } from '@mui/material';
import SubscribeDialog from './subscribeDialog';
import { useSelector } from "react-redux";
import { Event } from '../../../../shared/models/events';

const Recurrency = () => {
    const subscribeDialogRef = useRef<any>({});
    const event = useSelector((state: any) => state.selectedEvent as Event);
    const disabled = event.uuid ? false : true;

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