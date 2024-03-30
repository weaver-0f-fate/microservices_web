import React, { useEffect, useRef } from 'react';
import FullCalendar from "@fullcalendar/react";
import { Box } from "@mui/material"
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import { Provider as EventProvider } from './store/store';
import useSetEventUuid from './store/hooks/eventDetails/useSetEventUuid';
import useSetEvents from './store/hooks/useSetEvents';
import useEvent from './store/hooks/useEvent';
import timeGridPlugin from '@fullcalendar/timegrid';
import rrulePlugin from '@fullcalendar/rrule'
import EventDialog from './components/eventDialog';
import './styles/eventsCalendar.css';
import { useGetEvents } from './fetch/useGetEvents';
import DisplayFilters from './components/filters/displayFilters';
import CreateEventDialog from './components/createEventDialog';
import Search from './components/search';

export interface EventBaseInfo {
    title: string;
    rrule?: string,
    date: Date;
    category: string,
    place: string,
    uuid: string;
}

const EventsCalendar = () => {
    const setEventUuid = useSetEventUuid();
    const setEvents = useSetEvents();
    const eventStore = useEvent();
    const getEvents = useGetEvents();
    const eventDialogRef = useRef<any>();
    const createEventDialogRef = useRef<any>();

    useEffect(() => {
        getEvents(eventStore.filters.category, eventStore.filters.place, eventStore.filters.time)
            .then((result: any) => {
                console.log(result)
                setEvents(result);
            })

    }, [eventStore.filters.category, eventStore.filters.place, eventStore.filters.time])

    const handleEventClick = (event: any) => {
        setEventUuid(event.event.extendedProps.uuid);
        eventDialogRef.current.openDialog();
    }

    return (
        <Box display="flex" flexDirection="column">
            <Search openDialog={() => eventDialogRef.current.openDialog()} />
            <Box display="flex" flexDirection="row" >
                <DisplayFilters />
                <Box p={2} width={'100%'}>
                    <FullCalendar
                        plugins={[dayGridPlugin, interactionPlugin, timeGridPlugin, rrulePlugin]}
                        eventClick={handleEventClick}
                        initialView="dayGridMonth"
                        events={eventStore.events}
                        eventTimeFormat={{
                            hour: '2-digit',
                            minute: '2-digit',
                            hour12: false
                        }}
                        headerToolbar={{
                            left: 'prev,next today create',
                            center: 'title',
                            right: 'dayGridMonth,timeGridWeek,timeGridDay',
                        }}
                        customButtons={{
                            create: {
                                text: 'Create',
                                click: () => createEventDialogRef.current.openDialog()
                            }
                        }}
                        />
                    <EventDialog ref={eventDialogRef}/>
                    <CreateEventDialog ref={createEventDialogRef} />
                </Box>
            </Box>
        </Box>
    )
}

const EventsCalendarWithProvider = () => {
    return (
        <EventProvider>
            <EventsCalendar />
        </EventProvider>
    )
}

export default EventsCalendarWithProvider;