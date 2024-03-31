import React, { forwardRef, useEffect, useImperativeHandle, useState } from 'react';
import { Grid, Dialog, DialogTitle, DialogContent, Button, DialogActions, Container } from '@mui/material';
import Frequency from './frequency';
import Interval from './interval';
import StartDate from './startDate';
import EndDate from './endDate';
import { RRule } from 'rrule'
import { useUpdateEvent } from '../../fetch/useUpdateEvent';
import { useDispatch, useSelector } from "react-redux";
import { Event } from '../../../../shared/models/events';
import { setSelectedEventRecurrency } from '../../store/selectedEventSlice';
import { setEventRecurrency } from '../../store/eventsSlice';

const RecurrencyDialog = forwardRef((props: any, ref: any) => {
    const [openDialog, setOpenDialog] = useState<boolean>(false);
    const event = useSelector((state: any) => state.selectedEvent as Event);
    const dispatch = useDispatch();
    
    const [frequency, setFrequency] = useState<number>(3);
    const [interval, setInterval] = useState<number>(1);
    const [startDate, setStartDate] = useState<Date>(new Date());
    const [endDate, setEndDate] = useState<Date | null>(null);
    const updateEvent = useUpdateEvent();

    useEffect(() => {
        if (event.recurrency) {
            const rrule = RRule.fromString(event.recurrency);
            setFrequency(rrule.options.freq);
            setInterval(rrule.options.interval);
            setStartDate(rrule.options.dtstart);
            setEndDate(rrule.options.until);
        }
    }, [event.recurrency])

    useImperativeHandle(ref, () => ({
        openDialog() {
            setOpenDialog(true);
        },
    }))

    const updateRecurrency = (eventUuid: string, recurrency?: string, ) => {
        dispatch(setSelectedEventRecurrency(recurrency))
        dispatch(setEventRecurrency({eventUuid, recurrency}));
    }

    const handleAddRecurrency = () => {
        const rule = new RRule({
            freq: frequency,
            interval: interval,
            dtstart: startDate,
            until: endDate
        })
        const rrule = rule.toString();

        updateEvent(event.uuid, '/recurrency', rrule)
        .then(() => {
            updateRecurrency(event.uuid, rrule);
        })
        
        setOpenDialog(false);
    }

    const handleClose = () => {
        setOpenDialog(false)
    };

    const handleRemoveRecurrency = () => {
        updateEvent(event.uuid, '/recurrency', '')
        .then(() => {
            updateRecurrency(event.uuid, undefined);
        })
        setOpenDialog(false)
    }

    return (
        <Dialog open={openDialog}>
            <DialogTitle>
                Add Recurrency to event: {event.title}
            </DialogTitle>
            <DialogContent>
                <Container maxWidth="sm" sx={{ p: 5 }}>
                    <Grid container spacing={2}>
                        <Frequency setFrequency={setFrequency} defaultValue={frequency}/>
                        <Interval setInterval={setInterval} defaultValue={interval}/>
                        <StartDate setStartDate={setStartDate} defaultValue={startDate}/>
                        <EndDate setEndDate={setEndDate} defaultValue={endDate}/>
                    </Grid>
                </Container>
            </DialogContent>
            <DialogActions>
                <Button onClick={handleClose}>Cancel</Button>
                <Button onClick={handleRemoveRecurrency}>Remove Recurrency</Button>
                <Button onClick={handleAddRecurrency}>Save</Button>
            </DialogActions>
        </Dialog>
    )
})

export default RecurrencyDialog