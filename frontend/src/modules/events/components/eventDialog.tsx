import React from "react";
import { Button, Dialog, DialogActions, DialogContent, DialogTitle } from "@mui/material"
import EventDetails from "./eventDetails"
import { forwardRef, useState, useImperativeHandle } from "react";
import DeleteEvent from "./deleteEvent";
import Recurrency from "./recurrency/recurrency";
import Subscribe from "./subscription/subscribe";
import { useSelector } from "react-redux";
import { Event } from '../../../shared/models/events';


const EventDialog = forwardRef((props: any, ref: any) => {
    const event = useSelector((state: any) => state.selectedEvent as Event);
    const [openDialog, setOpenDialog] = useState<boolean>(false);

    useImperativeHandle(ref, () => ({
        openDialog() {
            setOpenDialog(true);
        },
    }))

    const handleClose = () => {
        setOpenDialog(false);
    }

    return (
        <Dialog open={openDialog}>
            <DialogTitle>{event.title}</DialogTitle>
            <DialogContent>
                <EventDetails />
            </DialogContent>
            <DialogActions>
                <Subscribe />
                <Recurrency />
                <DeleteEvent closeDetails={handleClose}/>
                <Button onClick={handleClose}>Close</Button>
            </DialogActions>
        </Dialog>
    )
})

export default EventDialog;