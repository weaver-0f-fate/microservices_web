import { Button, Container, Dialog, DialogActions, DialogContent, DialogTitle, Grid, InputLabel, TextField } from "@mui/material";
import React, { forwardRef, useState, useImperativeHandle, useRef } from "react";
import NotificationDate from "./notificationTime";
import { useSubscribeForEvent } from "../../fetch/useSubscribeForEvent";
import { useSelector } from "react-redux";
import { SelectedEventState } from "../../store/selectedEventSlice";


const SubscribeDialog = forwardRef((props: any, ref: any) => {
    const [openDialog, setOpenDialog] = useState<boolean>(false);
    const event = useSelector((state: any) => state.selectedEvent as SelectedEventState);

    const [email, setEmail] = useState<string>('');
    const [notificationDate, setNotificationDate] = useState<Date>(new Date());

    const inputRef = useRef<any>();
    const [isValidEmail, setIsValidEmail] = useState<boolean>(true);

    const handleValueChange = (event: any) => {
        console.log(event.target.value)
        setEmail(event.target.value);
        
        // Regular expression to validate email format
        const emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;

        // Check if the entered email matches the pattern
        setIsValidEmail(emailPattern.test(event.target.value));
    };

    const subscribe = useSubscribeForEvent();

    useImperativeHandle(ref, () => ({
        openDialog() {
            setOpenDialog(true);
        },
    }))
    
    const handleClose = () => {
        setOpenDialog(false)
    };

    const handleSubscribe = () => {
        subscribe(event.event.uuid, email, notificationDate)
        setOpenDialog(false)
    }

    return (
        <Dialog open={openDialog}>
            <DialogTitle>
                Subscribe to event: {event.event.title}
            </DialogTitle>
            <DialogContent>
                <Container maxWidth="sm" sx={{ p: 5 }}>
                    <Grid container spacing={2}>
                        <Grid item xs={12}>
                            <InputLabel htmlFor="email-input">Email</InputLabel>
                            <TextField
                                id="email-input"
                                fullWidth
                                type='email'
                                defaultValue={email}
                                error={!isValidEmail}
                                inputRef={inputRef}
                                onChange={handleValueChange}
                                inputProps={{ min: 0, max: 10, step: 1 }}
                            />
                        </Grid>
                        <NotificationDate setNotificationDate={setNotificationDate} defaultValue={notificationDate}/>
                    </Grid>
                </Container>
            </DialogContent>
            <DialogActions>
                <Button onClick={handleClose}>Cancel</Button>
                <Button onClick={handleSubscribe} disabled={!isValidEmail}>Subscribe</Button>
            </DialogActions>
        </Dialog>
    )
})

export default SubscribeDialog;