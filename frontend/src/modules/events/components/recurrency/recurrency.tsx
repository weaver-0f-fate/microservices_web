import React, { useRef } from 'react';
import { Button } from '@mui/material';
import RecurrencyDialog from './recurrencyDialog';
import { useSelector } from "react-redux";
import { SelectedEventState } from '../../store/selectedEventSlice';

const Recurrency = () => {
    const AddRecurrencyDialogRef = useRef<any>({});
    const event = useSelector((state: any) => state.selectedEvent as SelectedEventState);
    const disabled = event.event.uuid ? false : true;

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