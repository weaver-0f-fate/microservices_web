import React, { forwardRef, useImperativeHandle, useRef, useState } from "react";
import { Button, Dialog, DialogActions, DialogTitle } from "@mui/material";
import { useDispatch, useSelector } from "react-redux";
import { removeEvent } from "../store/eventsSlice";
import { SelectedEventState, setSelectedEvent } from "../store/selectedEventSlice";
import { useDeleteEvent } from "../fetch/useDeleteEvent";

interface DeleteEventProps {
    closeDetails: () => void;
}

const DeleteEvent = (props: DeleteEventProps) => {
    const { closeDetails } = props;
    const event = useSelector((state: any) => state.selectedEvent as SelectedEventState);
        
    const disabled = event && event.event.uuid ? false : true;
    const confirmDeleteDialog = useRef<any>({});

    const handleDelete = () => {
        confirmDeleteDialog.current.openDialog();
    }

    return (
        <>
        <Button 
            sx={{ ml: 2 }} 
            onClick={handleDelete}
            disabled={disabled}>
            Delete
        </Button>
        <DeleteDialog ref={confirmDeleteDialog} closeDetails={closeDetails}/>
        </>
    )
}

const DeleteDialog = forwardRef((props: any, ref: any) => {
    const { closeDetails } = props;
    const event = useSelector((state: any) => state.selectedEvent as SelectedEventState);
    const dispatch = useDispatch();
    const [openDialog, setOpenDialog] = useState<boolean>(false);
    const deleteEvent = useDeleteEvent();

    useImperativeHandle(ref, () => ({
        openDialog() {
            setOpenDialog(true);
        },
    }))

    const handleDelete = () => {
        if(event)
            deleteEvent(event.event.uuid)
                .then(() => {
                    if(event)
                    {
                        dispatch(setSelectedEvent(undefined));
                        dispatch(removeEvent(event.event.uuid));
                    }
                })
            setOpenDialog(false)
            closeDetails();
    }

    const handleClose = () => {
        setOpenDialog(false)
    };


    return (
        <Dialog open={openDialog} sx={{ minWidth: 300, minHeight: 50 }}>
            <DialogTitle>Delte event: {event.event ? event.event.title : ''}</DialogTitle>
            <DialogActions>
                <Button variant="contained" onClick={handleDelete}>
                    Delete
                </Button>
                <Button variant="contained" onClick={handleClose}>
                    Cancel
                </Button>
            </DialogActions>
        </Dialog>
    )
})

export default DeleteEvent;