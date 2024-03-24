import React, { forwardRef, useEffect, useImperativeHandle, useState } from 'react';
import { Grid, Dialog, DialogTitle, DialogContent, Button, DialogActions, Container } from '@mui/material';
import useEvent from '../../store/hooks/useEvent';
import Frequency from './frequency';
import Interval from './interval';
import StartDate from './startDate';
import EndDate from './endDate';
import { RRule } from 'rrule'
import useSetRecurrency from '../../store/hooks/eventDetails/useSetRecurrency';
import { useUpdateEvent } from '../../fetch/useUpdateEvent';

const RecurrencyDialog = forwardRef((props: any, ref: any) => {
    const eventStore = useEvent();
    const [openDialog, setOpenDialog] = useState<boolean>(false);
    const setRecurrency = useSetRecurrency();
    
    const [frequency, setFrequency] = useState<number>(3);
    const [interval, setInterval] = useState<number>(1);
    const [startDate, setStartDate] = useState<Date>(new Date());
    const [endDate, setEndDate] = useState<Date | null>(null);
    const updateEvent = useUpdateEvent();

    useEffect(() => {
        if (eventStore.selectedEvent.recurrency) {
            const rrule = RRule.fromString(eventStore.selectedEvent.recurrency);
            setFrequency(rrule.options.freq);
            setInterval(rrule.options.interval);
            setStartDate(rrule.options.dtstart);
            setEndDate(rrule.options.until);
        }
    }, [eventStore.selectedEvent.recurrency])

    useImperativeHandle(ref, () => ({
        openDialog() {
            setOpenDialog(true);
        },
    }))

    const handleAddRecurrency = () => {
        const rule = new RRule({
            freq: frequency,
            interval: interval,
            dtstart: startDate,
            until: endDate
        })
        const rrule = rule.toString();

        updateEvent(eventStore.selectedEvent.uuid, '/recurrency', rrule)
        .then(() => {
            setRecurrency(eventStore.selectedEvent.uuid, rrule);
        })
        
        setOpenDialog(false);
    }

    const handleClose = () => {
        setOpenDialog(false)
    };

    const handleRemoveRecurrency = () => {
        updateEvent(eventStore.selectedEvent.uuid, '/recurrency', '')
        .then(() => {
            setRecurrency(eventStore.selectedEvent.uuid, undefined);
        })
        setOpenDialog(false)
    }

    return (
        <Dialog open={openDialog}>
            <DialogTitle>
                Add Recurrency to event: {eventStore.selectedEvent.title}
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