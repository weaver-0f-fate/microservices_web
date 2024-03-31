import React, { useRef } from 'react';
import { Button } from '@mui/material';
import RecurrencyDialog from './recurrencyDialog';
import { useSelector } from "react-redux";
import { Event } from '../../../../shared/models/events';

const Recurrency = () => {
    const AddRecurrencyDialogRef = useRef<any>({});
    const event = useSelector((state: any) => state.selectedEvent as Event);
    const disabled = event.uuid ? false : true;

    const handleRecurrency = () => {
        AddRecurrencyDialogRef.current.openDialog();
    }

    return (
        <>
            <Button onClick={handleRecurrency} disabled={disabled}>
                Configure Recurrency
            </Button>
            <RecurrencyDialog ref={AddRecurrencyDialogRef} />
        </>
    )
}

export default Recurrency;